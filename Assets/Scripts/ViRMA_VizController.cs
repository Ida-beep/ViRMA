﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViRMA_VizController : MonoBehaviour
{
    /* --- public --- */

    // actions
    public SteamVR_ActionSet cellNavigationControls;
    public SteamVR_Action_Boolean cellNavigationToggle;

    // cells and axes objects
    [HideInInspector] public List<Cell> cellData;
    [HideInInspector] public List<GameObject> cellObjs, axisXPointObjs, axisYPointObjs, axisZPointObjs;
    [HideInInspector] public LineRenderer axisXLine, axisYLine, axisZLine;

    /*--- private --- */

    // general
    private Rigidbody rigidBody;
    private float previousDistanceBetweenHands;
    private Bounds cellsAndAxesBounds;

    // cell properties
    private GameObject cellsandAxesWrapper;
    private float maxParentScale = 1.0f;
    private float minParentScale = 0.1f;
    private float defaultParentSize = 0.2f;
    private float defaultCellSpacingRatio = 1.5f;
    private string cellMaterial = "Materials/UnlitCell";

    private void Awake()
    {
        // setup cells and axes wrapper
        cellsandAxesWrapper = new GameObject("CellsAndAxesWrapper");

        // setup rigidbody
        gameObject.AddComponent<Rigidbody>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.drag = 0.1f;
        rigidBody.angularDrag = 0.5f;       
    }
    private IEnumerator Start()
    {
        // dummy queries for debugging
        Query dummyQuery = new Query();
        dummyQuery.SetAxis("X", 3, "Tagset");
        dummyQuery.SetAxis("Y", 7, "Tagset");
        dummyQuery.SetAxis("Z", 77, "Hierarchy");
        //dummyQuery.SetAxis("X", 7, "Tagset");
        //dummyQuery.SetAxis("Y", 7, "Tagset");
        //dummyQuery.SetAxis("Z", 7, "Tagset");
        //dummyQuery.AddFilter(115, "Hierarchy");
        //dummyQuery.AddFilter(116, "Hierarchy");

        // get actual data from server
        yield return StartCoroutine(ViRMA_APIController.GetCells(dummyQuery, (cells) => {
            cellData = cells;
        }));

        // generate textures and texture arrays from local image storage
        GenerateTexturesAndTextureArrays(cellData);

        // generate cell X, Y, Z axes 
        GenerateAxes(cellData);

        // generate cells and their posiitons, centered on a parent
        GenerateCells(cellData);

        // set center point of wrapper around cells and axes
        CenterParentOnCellsAndAxes();

        // calculate bounding box to set cells positional limits
        CalculateCellsAndAxesBounds();

        // show cells/axes bounds and bounds center for debugging
        // ToggleDebuggingBounds(); // debugging

        // add cells and axes to final parent to set default starting scale and position
        SetupDefaultScaleAndPosition();

        // so it does not affect bounds, organise hierarachy after everything is rendered
        OrganiseHierarchy();

        // activate navigation action controls
        cellNavigationControls.Activate();
    }
    private void Update()
    {
        // control call navigation and spatial limitations
        CellNavigationController();
        CellNavigationLimiter();

        // draw axes line renderers 
        DrawAxesLines();
    }


    // cell and axes generation
    private void GenerateTexturesAndTextureArrays(List<Cell> cellData)
    {
        // make a list of all the unique image textures present in the current query
        List<KeyValuePair<string, Texture2D>> uniqueTextures = new List<KeyValuePair<string, Texture2D>>();
        foreach (var newCell in cellData)
        {
            if (!newCell.Filtered)
            {
                int index = uniqueTextures.FindIndex(a => a.Key == newCell.ImageName);
                if (index == -1)
                {
                    byte[] imageBytes = File.ReadAllBytes(ViRMA_APIController.imagesDirectory + newCell.ImageName);
                    newCell.ImageTexture = ConvertImage(imageBytes);
                    KeyValuePair<string, Texture2D> uniqueTexture = new KeyValuePair<string, Texture2D>(newCell.ImageName, newCell.ImageTexture);
                    uniqueTextures.Add(uniqueTexture);
                }
                else
                {
                    newCell.ImageTexture = uniqueTextures[index].Value;
                }
            }
        }

        // calculate the number of texture arrays needed based on the size of the first texture in the list
        int textureWidth = uniqueTextures[0].Value.width; // e.g. 1024 or 684
        int textureHeight = uniqueTextures[0].Value.height; // e.g. 765 or 485
        int maxTextureArraySize = SystemInfo.maxTextureSize; // e.g. 16384 (most GPUs)
        int maxTexturesPerArray = maxTextureArraySize / textureHeight; // e.g. 22 or 33 
        int totalTextureArrays = uniqueTextures.Count / maxTexturesPerArray + 1;

        for (int i = 0; i < totalTextureArrays; i++)
        {
            //Debug.Log("----------------- " + i + " -----------------"); // debugging

            if (i != totalTextureArrays - 1)
            {
                Material newtextureArrayMaterial = new Material(Resources.Load(cellMaterial) as Material);
                Texture2D newTextureArray = new Texture2D(textureWidth, textureHeight * maxTexturesPerArray, TextureFormat.DXT1, false);
                for (int j = 0; j < maxTexturesPerArray; j++)
                {
                    int uniqueTextureIndex = j + maxTexturesPerArray * i;
                    Texture2D targetTexture = uniqueTextures[uniqueTextureIndex].Value;
                    if (targetTexture.width != textureWidth || targetTexture.height != textureHeight)
                    {
                        Debug.LogError("Texture " + uniqueTextures[uniqueTextureIndex].Key + " is not " + textureWidth + " x " + textureHeight + " and so will not fit properly in the texture array!");
                    }
                    Graphics.CopyTexture(targetTexture, 0, 0, 0, 0, targetTexture.width, targetTexture.height, newTextureArray, 0, 0, 0, targetTexture.height * j);

                    //Debug.Log(j + " | " + uniqueTextureIndex); // debugging

                    foreach (var cellDataObj in this.cellData)
                    {
                        if (cellDataObj.ImageName == uniqueTextures[uniqueTextureIndex].Key)
                        {
                            cellDataObj.TextureArrayId = j;
                            cellDataObj.TextureArrayMaterial = newtextureArrayMaterial;
                            cellDataObj.TextureArraySize = maxTexturesPerArray;
                        }
                    }
                }
                newtextureArrayMaterial.mainTexture = newTextureArray;
            }
            else
            {
                Material newtextureArrayMaterial = new Material(Resources.Load(cellMaterial) as Material);
                int lastTextureArraySize = uniqueTextures.Count - (maxTexturesPerArray * (totalTextureArrays - 1));
                Texture2D newTextureArray = new Texture2D(textureWidth, textureHeight * lastTextureArraySize, TextureFormat.DXT1, false);
                for (int j = 0; j < lastTextureArraySize; j++)
                {
                    int uniqueTextureIndex = j + maxTexturesPerArray * i;
                    Texture2D targetTexture = uniqueTextures[uniqueTextureIndex].Value;
                    if (targetTexture.width != textureWidth || targetTexture.height != textureHeight)
                    {
                        Debug.LogError("Texture " + uniqueTextures[uniqueTextureIndex].Key + " is not " + textureWidth + " x " + textureHeight + " and so will not fit properly in the texture array!");
                    }
                    Graphics.CopyTexture(targetTexture, 0, 0, 0, 0, targetTexture.width, targetTexture.height, newTextureArray, 0, 0, 0, targetTexture.height * j);

                    // Debug.Log(j + " | " + uniqueTextureIndex); // debugging

                    foreach (var cellDataObj in this.cellData)
                    {
                        if (cellDataObj.ImageName == uniqueTextures[uniqueTextureIndex].Key)
                        {
                            cellDataObj.TextureArrayId = j;
                            cellDataObj.TextureArrayMaterial = newtextureArrayMaterial;
                            cellDataObj.TextureArraySize = lastTextureArraySize;
                        }
                    }
                }
                newtextureArrayMaterial.mainTexture = newTextureArray;
            }
        }
    }
    private static Texture2D ConvertImage(byte[] ddsBytes)
    {
        byte ddsSizeCheck = ddsBytes[4];
        if (ddsSizeCheck != 124)
        {
            throw new Exception("Invalid DDS DXTn texture size! (not 124)");
        }
        int height = ddsBytes[13] * 256 + ddsBytes[12];
        int width = ddsBytes[17] * 256 + ddsBytes[16];

        int ddsHeaderSize = 128;
        byte[] dxtBytes = new byte[ddsBytes.Length - ddsHeaderSize];
        Buffer.BlockCopy(ddsBytes, ddsHeaderSize, dxtBytes, 0, ddsBytes.Length - ddsHeaderSize);
        Texture2D texture = new Texture2D(width, height, TextureFormat.DXT1, false);

        texture.LoadRawTextureData(dxtBytes);
        texture.Apply();
        return (texture);
    }
    private void GenerateCells(List<Cell> cellData)
    {
        // grab cell prefab from resoucres
        GameObject cellPrefab = Resources.Load("Prefabs/CellPrefab") as GameObject;

        // loop through all cells data from server
        foreach (var newCellData in cellData)
        {
            // create a primitive cube and set its scale to match image aspect ratio
            GameObject cell = Instantiate(cellPrefab);       
            cell.AddComponent<ViRMA_Cell>().thisCellData = newCellData;

            // adjust aspect ratio
            float aspectRatio = 1.5f;
            cell.transform.localScale = new Vector3(aspectRatio, 1, 1);

            // assign coordinates to cell from server using a pre-defined space multiplier
            Vector3 nodePosition = new Vector3(newCellData.Coordinates.x, newCellData.Coordinates.y, newCellData.Coordinates.z) * (defaultCellSpacingRatio + 1);
            cell.transform.position = nodePosition;
            cell.transform.parent = cellsandAxesWrapper.transform;

            // name cell object and add it to a list of objects for reference
            cell.name = "Cell(" + newCellData.Coordinates.x + "," + newCellData.Coordinates.y + "," + newCellData.Coordinates.z + ")";
            cellObjs.Add(cell);
        }
    }
    private void GenerateAxes(List<Cell> cells)
    {
        // get max cell axis values
        float maxX = 0;
        float maxY = 0;
        float maxZ = 0;
        foreach (var newCell in cells)
        {
            if (newCell.Coordinates.x > maxX)
            {
                maxX = newCell.Coordinates.x;
            }
            if (newCell.Coordinates.y > maxY)
            {
                maxY = newCell.Coordinates.y;
            }
            if (newCell.Coordinates.z > maxZ)
            {
                maxZ = newCell.Coordinates.z;
            }
        }

        // reuse same material and just change colour property
        Material transparentMaterial = Resources.Load("Materials/BasicTransparent") as Material;
        MaterialPropertyBlock materialProperties = new MaterialPropertyBlock();
        Color32 transparentRed = new Color32(255, 0, 0, 130);
        Color32 transparentGreen = new Color32(0, 255, 0, 130);
        Color32 transparentBlue = new Color32(0, 0, 255, 130);

        // origin
        GameObject AxisOriginPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
        AxisOriginPoint.GetComponent<Renderer>().material = transparentMaterial;
        materialProperties.SetColor("_Color", new Color32(0, 0, 0, 255));
        AxisOriginPoint.GetComponent<Renderer>().SetPropertyBlock(materialProperties);
        AxisOriginPoint.name = "AxisOriginPoint";
        AxisOriginPoint.transform.position = Vector3.zero;
        AxisOriginPoint.transform.localScale = Vector3.one * 0.5f;
        AxisOriginPoint.transform.parent = cellsandAxesWrapper.transform;
        axisXPointObjs.Add(AxisOriginPoint);
        axisYPointObjs.Add(AxisOriginPoint);
        axisZPointObjs.Add(AxisOriginPoint);

        // x axis
        GameObject AxisXLineObj = new GameObject("AxisXLine");
        axisXLine = AxisXLineObj.AddComponent<LineRenderer>();
        axisXLine.GetComponent<Renderer>().material = transparentMaterial;
        materialProperties.SetColor("_Color", transparentRed);
        axisXLine.GetComponent<Renderer>().SetPropertyBlock(materialProperties);
        axisXLine.positionCount = 2;
        axisXLine.startWidth = 0.05f;
        for (int i = 0; i <= maxX; i++)
        {
            GameObject AxisXPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            AxisXPoint.GetComponent<Renderer>().material = transparentMaterial;
            AxisXPoint.GetComponent<Renderer>().SetPropertyBlock(materialProperties);
            AxisXPoint.name = "AxisXPoint" + i;
            AxisXPoint.transform.position = new Vector3(i, 0, 0) * (defaultCellSpacingRatio + 1);
            AxisXPoint.transform.localScale = Vector3.one * 0.5f;
            AxisXPoint.transform.parent = cellsandAxesWrapper.transform;
            axisXPointObjs.Add(AxisXPoint);
        }

        // y axis
        GameObject AxisYLineObj = new GameObject("AxisYLine");
        axisYLine = AxisYLineObj.AddComponent<LineRenderer>();
        axisYLine.GetComponent<Renderer>().material = transparentMaterial;
        materialProperties.SetColor("_Color", transparentGreen);
        axisYLine.GetComponent<Renderer>().SetPropertyBlock(materialProperties);

        axisYLine.positionCount = 2;
        axisYLine.startWidth = 0.05f;
        for (int i = 0; i <= maxY; i++)
        {
            GameObject AxisYPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            AxisYPoint.GetComponent<Renderer>().material = transparentMaterial;
            AxisYPoint.GetComponent<Renderer>().SetPropertyBlock(materialProperties);
            AxisYPoint.name = "AxisYPoint" + i;
            AxisYPoint.transform.position = new Vector3(0, i, 0) * (defaultCellSpacingRatio + 1);
            AxisYPoint.transform.localScale = Vector3.one * 0.5f;
            AxisYPoint.transform.parent = cellsandAxesWrapper.transform;
            axisYPointObjs.Add(AxisYPoint);
        }

        // z axis
        GameObject AxisZLineObj = new GameObject("AxisZLine");
        axisZLine = AxisZLineObj.AddComponent<LineRenderer>();
        axisZLine.GetComponent<Renderer>().material = transparentMaterial;
        materialProperties.SetColor("_Color", transparentBlue);
        axisZLine.GetComponent<Renderer>().SetPropertyBlock(materialProperties);
        axisZLine.positionCount = 2;
        axisZLine.startWidth = 0.05f;
        for (int i = 0; i <= maxZ; i++)
        {
            GameObject AxisZPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            AxisZPoint.GetComponent<Renderer>().material = transparentMaterial;
            AxisZPoint.GetComponent<Renderer>().SetPropertyBlock(materialProperties);
            AxisZPoint.name = "AxisZPoint" + i;
            AxisZPoint.transform.position = new Vector3(0, 0, i) * (defaultCellSpacingRatio + 1);
            AxisZPoint.transform.localScale = Vector3.one * 0.5f;
            AxisZPoint.transform.parent = cellsandAxesWrapper.transform;
            axisZPointObjs.Add(AxisZPoint);
        }
    }
    private void DrawAxesLines()
    {
        // x axis
        if (axisXLine)
        {
            if (axisXPointObjs.Count > 1)
            {
                axisXLine.SetPosition(0, axisXPointObjs[0].transform.position);
                axisXLine.SetPosition(1, axisXPointObjs[axisXPointObjs.Count - 1].transform.position);
            }
        }

        // y axis
        if (axisYLine)
        {
            if (axisYPointObjs.Count > 1)
            {
                axisYLine.SetPosition(0, axisYPointObjs[0].transform.position);
                axisYLine.SetPosition(1, axisYPointObjs[axisYPointObjs.Count - 1].transform.position);
            }
        }

        // z axis
        if (axisZLine)
        {
            if (axisZPointObjs.Count > 1)
            {
                axisZLine.SetPosition(0, axisZPointObjs[0].transform.position);
                axisZLine.SetPosition(1, axisZPointObjs[axisZPointObjs.Count - 1].transform.position);
            }
        }
    }
    private void OrganiseHierarchy()
    {
        // add cells to hierarchy parent
        GameObject cellsParent = new GameObject("Cells");
        cellsParent.transform.parent = cellsandAxesWrapper.transform;
        foreach (var cell in cellObjs)
        {
            cell.transform.parent = cellsParent.transform;
        }

        // add axes to hierarchy parent
        GameObject axesParent = new GameObject("Axes");
        axesParent.transform.parent = cellsandAxesWrapper.transform;
        foreach (GameObject point in axisXPointObjs)
        {
            point.transform.parent = axesParent.transform;
        }
        axisXLine.gameObject.transform.parent = axesParent.transform;
        foreach (GameObject point in axisYPointObjs)
        {
            point.transform.parent = axesParent.transform;
        }
        axisYLine.gameObject.transform.parent = axesParent.transform;
        foreach (GameObject point in axisZPointObjs)
        {
            point.transform.parent = axesParent.transform;
        }
        axisZLine.gameObject.transform.parent = axesParent.transform;
    }


    // node navigation (position, rotation, scale)
    private void CellNavigationController()
    {
        if (cellNavigationToggle[SteamVR_Input_Sources.LeftHand].state && cellNavigationToggle[SteamVR_Input_Sources.RightHand].state)
        {
            // both triggers held down
            ToggleCellScaling();
        }
        else if (cellNavigationToggle[SteamVR_Input_Sources.LeftHand].state || cellNavigationToggle[SteamVR_Input_Sources.RightHand].state)
        {
            // one trigger held down
            if (cellNavigationToggle[SteamVR_Input_Sources.RightHand].state)
            {
                // right trigger held down
                ToggleCellPositioning();
            }

            if (cellNavigationToggle[SteamVR_Input_Sources.LeftHand].state)
            {
                // left trigger held down
                ToggleCellRotation();
            }
        }
        else
        {
            // no triggers held down
            if (previousDistanceBetweenHands != 0)
            {
                previousDistanceBetweenHands = 0;
            }
        }    
    }
    private void ToggleCellPositioning()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        if (Player.instance.rightHand.GetTrackedObjectVelocity().magnitude > 0.5f)
        {
            /*      
            Vector3 localVelocity = transform.InverseTransformDirection(Player.instance.rightHand.GetTrackedObjectVelocity());
            localVelocity.x = 0;
            localVelocity.y = 0;
            localVelocity.z = 0;
            rigidBody.velocity = transform.TransformDirection(localVelocity) * 2f;
            */

            // scale throwing velocity with the size of the parent
            float parentMagnitude = transform.lossyScale.magnitude;
            float thrustAdjuster = parentMagnitude * 5f;
            Vector3 controllerVelocity = Player.instance.rightHand.GetTrackedObjectVelocity();
            rigidBody.velocity = controllerVelocity * thrustAdjuster;
        }
    }
    private void ToggleCellRotation()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        Vector3 localAngularVelocity = transform.InverseTransformDirection(Player.instance.leftHand.GetTrackedObjectAngularVelocity());
        localAngularVelocity.x = 0;
        //localAngularVelocity.y = 0;
        localAngularVelocity.z = 0;
        rigidBody.angularVelocity = transform.TransformDirection(localAngularVelocity) * 0.1f;
    }
    private void ToggleCellScaling()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        Vector3 leftHandPosition = Player.instance.leftHand.transform.position;
        Vector3 rightHandPosition = Player.instance.rightHand.transform.position;
        float thisFrameDistance = Mathf.Round(Vector3.Distance(leftHandPosition, rightHandPosition) * 100.0f) * 0.01f;

        if (previousDistanceBetweenHands == 0)
        {
            previousDistanceBetweenHands = thisFrameDistance;
        }
        else
        {
            if (thisFrameDistance > previousDistanceBetweenHands)
            {
                Vector3 targetScale = Vector3.one * maxParentScale;            
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 2f * Time.deltaTime);
            }
            if (thisFrameDistance < previousDistanceBetweenHands)
            {
                Vector3 targetScale = Vector3.one * minParentScale;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 2f * Time.deltaTime);
            }
            previousDistanceBetweenHands = thisFrameDistance;
        }

        // calculate bounding box again
        CalculateCellsAndAxesBounds();
    }
    private void CellNavigationLimiter()
    {
        if (Player.instance)
        {
            Vector3 currentVelocity = rigidBody.velocity;

            // x and z
            float boundary = Mathf.Max(Mathf.Max(cellsAndAxesBounds.size.x, cellsAndAxesBounds.size.y), cellsAndAxesBounds.size.z);
            if (Vector3.Distance(transform.position, Player.instance.hmdTransform.transform.position) > boundary)
            {
                Vector3 normalisedDirection = (transform.position - Player.instance.hmdTransform.transform.position).normalized;
                Vector3 v = rigidBody.velocity;
                float d = Vector3.Dot(v, normalisedDirection);
                if (d > 0f) v -= normalisedDirection * d;
                rigidBody.velocity = v;
            }

            // y max
            float maxDistanceY = Player.instance.eyeHeight + cellsAndAxesBounds.extents.y;
            if (transform.position.y >= maxDistanceY && currentVelocity.y > 0)
            {
                currentVelocity.y = 0;
                rigidBody.velocity = currentVelocity;
            }

            // y min
            float minDistanceY = Player.instance.eyeHeight - cellsAndAxesBounds.extents.y;
            if (transform.position.y <= minDistanceY && currentVelocity.y < 0)
            {
                currentVelocity.y = 0;
                rigidBody.velocity = currentVelocity;
            }

        }
    }


    // general  
    private void CalculateCellsAndAxesBounds()
    {
        // calculate bounding box
        Renderer[] meshes = cellsandAxesWrapper.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(cellsandAxesWrapper.transform.position, Vector3.zero);
        foreach (Renderer mesh in meshes)
        {
            bounds.Encapsulate(mesh.bounds);
        }
        cellsAndAxesBounds = bounds;
    }
    private void SetupDefaultScaleAndPosition()
    {
        // set wrapper position and parent cells/axes to wrapper and set default starting scale
        transform.position = cellsAndAxesBounds.center;
        cellsandAxesWrapper.transform.parent = transform;
        transform.localScale = Vector3.one * defaultParentSize;

        // get the bounds of the newly resized cells/axes
        Renderer[] meshes = GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer mesh in meshes)
        {
            bounds.Encapsulate(mesh.bounds);
        }

        // calculate distance to place cells/axes in front of player based on longest axis
        float distance = Mathf.Max(Mathf.Max(bounds.size.x, bounds.size.y), bounds.size.z);
        Vector3 flattenedVector = Player.instance.bodyDirectionGuess;
        flattenedVector.y = 0;
        flattenedVector.Normalize();
        Vector3 spawnPos = Player.instance.hmdTransform.position + flattenedVector * distance;
        transform.position = spawnPos;
        transform.LookAt(2 * transform.position - Player.instance.hmdTransform.position);

        // recalculate bounds to dertmine positional limits 
        CalculateCellsAndAxesBounds();
    }
    private void CenterParentOnCellsAndAxes()
    {
        Transform[] children = cellsandAxesWrapper.transform.GetComponentsInChildren<Transform>();
        Vector3 newPosition = Vector3.one;
        foreach (var child in children)
        {
            newPosition += child.position;
            child.parent = null;
        }
        newPosition /= children.Length;
        cellsandAxesWrapper.transform.position = newPosition;
        foreach (var child in children)
        {
            child.parent = cellsandAxesWrapper.transform;
        }
    }


    // testing and debugging
    private void ToggleDebuggingBounds()
    {
        // show bounds in-game for debugging
        GameObject debugBounds = GameObject.CreatePrimitive(PrimitiveType.Cube);
        debugBounds.name = "DebugBounds"; 
        Destroy(debugBounds.GetComponent<Collider>());
        debugBounds.GetComponent<Renderer>().material = Resources.Load("Materials/BasicTransparent") as Material;
        debugBounds.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 130);
        debugBounds.transform.position = cellsAndAxesBounds.center;
        debugBounds.transform.localScale = cellsAndAxesBounds.size;
        debugBounds.transform.SetParent(cellsandAxesWrapper.transform);
        debugBounds.transform.SetAsFirstSibling();

        // show center of bounds in-game for debugging
        GameObject debugBoundsCenter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        debugBoundsCenter.name = "DebugBoundsCenter";       
        Destroy(debugBoundsCenter.GetComponent<Collider>());
        debugBoundsCenter.GetComponent<Renderer>().material = Resources.Load("Materials/BasicTransparent") as Material;
        debugBoundsCenter.GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 255);
        debugBoundsCenter.transform.position = cellsAndAxesBounds.center;
        debugBoundsCenter.transform.rotation = cellsandAxesWrapper.transform.rotation;
        debugBoundsCenter.transform.parent = cellsandAxesWrapper.transform;
        debugBoundsCenter.transform.SetAsFirstSibling();
    }   
}

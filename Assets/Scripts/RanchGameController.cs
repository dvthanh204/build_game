using System.Collections.Generic;
using UnityEngine;

public sealed class RanchGameController : MonoBehaviour
{
    private readonly List<ChickenActor> chickens = new List<ChickenActor>();
    private readonly List<FarmPlot> plots = new List<FarmPlot>();
    private readonly List<GameObject> eggs = new List<GameObject>();
    private ChickenActor player;
    private Camera gameCamera;
    private Material grass;
    private Material soil;
    private Material white;
    private Material yellow;
    private Material red;
    private Material brown;
    private Material green;
    private Material gold;
    private Material growingCrop;
    private Material readyCrop;
    private int coins = 25;
    private int grain = 3;
    private int seeds = 3;
    private float eggTimer = 18f;
    private string status = "Chạm mặt đất để di chuyển. Hãy khám phá nông trại!";
    private bool stylesReady;
    private GUIStyle panelStyle;
    private GUIStyle labelStyle;
    private GUIStyle buttonStyle;
    private GUIStyle smallStyle;
    private Texture2D panelTexture;
    private Texture2D buttonTexture;

    private void Start()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        CreateMaterials();
        CreateWorld();
        CreatePlayer();
        CreateCamera();
        CreateChickens();
        CreatePlots();
        CreateEgg(new Vector3(-5.4f, 0.25f, 1.8f));
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        eggTimer -= Time.deltaTime;
        if (eggTimer <= 0f)
        {
            eggTimer = 32f;
            if (eggs.Count < 5)
            {
                CreateEgg(new Vector3(-5.4f, 0.25f, 1.8f));
            }
        }

        if (gameCamera != null)
        {
            Vector3 desired = player.transform.position + new Vector3(0f, 10f, -11f);
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, desired, Time.deltaTime * 3f);
            gameCamera.transform.LookAt(player.transform.position + Vector3.up * 0.5f);
        }

        HandleWorldTap();
    }

    private void CreateMaterials()
    {
        grass = MakeMaterial(new Color(0.31f, 0.63f, 0.34f));
        soil = MakeMaterial(new Color(0.34f, 0.21f, 0.12f));
        white = MakeMaterial(new Color(1f, 0.91f, 0.68f));
        yellow = MakeMaterial(new Color(1f, 0.68f, 0.17f));
        red = MakeMaterial(new Color(0.87f, 0.22f, 0.18f));
        brown = MakeMaterial(new Color(0.48f, 0.27f, 0.15f));
        green = MakeMaterial(new Color(0.19f, 0.48f, 0.25f));
        gold = MakeMaterial(new Color(1f, 0.77f, 0.18f), 0.18f);
        growingCrop = MakeMaterial(new Color(0.43f, 0.72f, 0.28f));
        readyCrop = MakeMaterial(new Color(1f, 0.77f, 0.18f), 0.1f);
    }

    private Material MakeMaterial(Color color, float metallic = 0f)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material material = new Material(shader);
        material.color = color;
        material.enableInstancing = true;
        if (material.HasProperty("_Metallic"))
        {
            material.SetFloat("_Metallic", metallic);
        }
        return material;
    }

    private void CreateWorld()
    {
        RenderSettings.ambientLight = new Color(0.72f, 0.79f, 0.83f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.72f, 0.84f, 0.88f);
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.012f;

        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Meadow";
        ground.transform.position = new Vector3(0f, -0.3f, 0f);
        ground.transform.localScale = new Vector3(32f, 0.5f, 32f);
        ground.GetComponent<Renderer>().sharedMaterial = grass;

        GameObject sunObject = new GameObject("Warm morning sun");
        Light sun = sunObject.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.15f;
        sun.color = new Color(1f, 0.91f, 0.75f);
        sun.shadows = LightShadows.Soft;
        sunObject.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

        CreateBuilding("Cozy Coop", new Vector3(-5.5f, 0.7f, 3f), new Vector3(3.2f, 1.4f, 2.6f), white, red);
        CreateBuilding("Feed Mill", new Vector3(6f, 0.65f, 3f), new Vector3(2.5f, 1.3f, 2.4f), white, yellow);
        CreateTree(new Vector3(-8f, 0f, -5f));
        CreateTree(new Vector3(8f, 0f, -6f));
        CreateTree(new Vector3(10f, 0f, 7f));
        CreateFenceLine(new Vector3(-10f, 0f, 0f), 9, true);
        CreateFenceLine(new Vector3(0f, 0f, 10f), 11, false);
    }

    private void CreateBuilding(string name, Vector3 position, Vector3 size, Material wall, Material roof)
    {
        GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
        building.name = name;
        building.transform.position = position;
        building.transform.localScale = size;
        building.GetComponent<Renderer>().sharedMaterial = wall;
        GameObject roofObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roofObject.name = name + " roof";
        roofObject.transform.position = position + Vector3.up * (size.y * 0.65f);
        roofObject.transform.localScale = new Vector3(size.x * 1.12f, 0.35f, size.z * 1.12f);
        roofObject.GetComponent<Renderer>().sharedMaterial = roof;
    }

    private void CreateTree(Vector3 position)
    {
        CreatePrimitive("Tree trunk", PrimitiveType.Cylinder, position + Vector3.up * 0.8f, new Vector3(0.35f, 0.8f, 0.35f), brown);
        CreatePrimitive("Tree canopy", PrimitiveType.Sphere, position + Vector3.up * 2f, new Vector3(1.6f, 1.8f, 1.6f), green);
    }

    private void CreateFenceLine(Vector3 start, int count, bool alongZ)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = alongZ ? new Vector3(0f, 0f, i * 1.7f) : new Vector3(i * 1.7f, 0f, 0f);
            CreatePrimitive("Fence post", PrimitiveType.Cube, start + offset + Vector3.up * 0.5f,
                new Vector3(0.18f, 1f, 0.18f), brown);
        }
    }

    private void CreatePlayer()
    {
        player = CreateChicken("Pip", true, new Vector3(0f, 0f, 0f));
    }

    private void CreateChickens()
    {
        CreateChicken("Sunny", false, new Vector3(-4f, 0f, 1f));
        CreateChicken("Pepper", false, new Vector3(-3.2f, 0f, 4f));
        CreateChicken("Mochi", false, new Vector3(2f, 0f, 4f));
        CreateChicken("Poppy", false, new Vector3(3.5f, 0f, -1f));
    }

    private ChickenActor CreateChicken(string displayName, bool isPlayer, Vector3 position)
    {
        GameObject root = new GameObject(displayName);
        root.name = displayName;
        root.transform.position = position;
        CapsuleCollider rootCollider = root.AddComponent<CapsuleCollider>();
        rootCollider.center = new Vector3(0f, 0.35f, 0f);
        rootCollider.radius = 0.35f;
        rootCollider.height = 0.7f;
        ChickenActor actor = root.AddComponent<ChickenActor>();
        actor.Initialize(displayName, isPlayer);

        GameObject body = CreateChildPrimitive("Body", PrimitiveType.Sphere, root.transform,
            new Vector3(0f, 0.32f, 0f), new Vector3(0.65f, 0.62f, 0.75f), white);
        GameObject head = CreateChildPrimitive("Head", PrimitiveType.Sphere, root.transform,
            new Vector3(0f, 0.65f, 0.28f), new Vector3(0.42f, 0.42f, 0.42f), white);
        CreateChildPrimitive("Beak", PrimitiveType.Cube, head.transform,
            new Vector3(0f, -0.06f, 0.38f), new Vector3(0.2f, 0.12f, 0.2f), yellow);
        CreateChildPrimitive("Comb", PrimitiveType.Sphere, head.transform,
            new Vector3(0f, 0.23f, 0.05f), new Vector3(0.2f, 0.2f, 0.12f), red);
        CreateChildPrimitive("Left eye", PrimitiveType.Sphere, head.transform,
            new Vector3(-0.13f, 0.04f, 0.18f), new Vector3(0.055f, 0.07f, 0.04f), brown);
        CreateChildPrimitive("Right eye", PrimitiveType.Sphere, head.transform,
            new Vector3(0.13f, 0.04f, 0.18f), new Vector3(0.055f, 0.07f, 0.04f), brown);
        CreateChildPrimitive("Left wing", PrimitiveType.Sphere, root.transform,
            new Vector3(-0.29f, 0.32f, 0f), new Vector3(0.18f, 0.27f, 0.39f), white);
        CreateChildPrimitive("Right wing", PrimitiveType.Sphere, root.transform,
            new Vector3(0.29f, 0.32f, 0f), new Vector3(0.18f, 0.27f, 0.39f), white);
        CreateChildPrimitive("Left foot", PrimitiveType.Cylinder, root.transform,
            new Vector3(-0.15f, 0.07f, 0.12f), new Vector3(0.08f, 0.07f, 0.12f), yellow);
        CreateChildPrimitive("Right foot", PrimitiveType.Cylinder, root.transform,
            new Vector3(0.15f, 0.07f, 0.12f), new Vector3(0.08f, 0.07f, 0.12f), yellow);
        actor.SetBody(body.transform);
        chickens.Add(actor);
        return actor;
    }

    private GameObject CreateChildPrimitive(string name, PrimitiveType type, Transform parent,
        Vector3 localPosition, Vector3 localScale, Material material)
    {
        GameObject child = GameObject.CreatePrimitive(type);
        child.name = name;
        child.transform.SetParent(parent, false);
        child.transform.localPosition = localPosition;
        child.transform.localScale = localScale;
        child.GetComponent<Renderer>().sharedMaterial = material;
        Collider collider = child.GetComponent<Collider>();
        if (collider != null)
        {
            Destroy(collider);
        }
        return child;
    }

    private void CreatePlots()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject plotObject = new GameObject("Crop plot " + (i + 1));
            plotObject.transform.position = new Vector3(-1.5f + i * 2f, 0f, 6f);
            FarmPlot plot = plotObject.AddComponent<FarmPlot>();
            plot.Initialize(soil, growingCrop, readyCrop);
            plots.Add(plot);
        }
    }

    private void CreateEgg(Vector3 position)
    {
        GameObject egg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        egg.name = "Fresh egg";
        egg.transform.position = position;
        egg.transform.localScale = new Vector3(0.3f, 0.4f, 0.3f);
        egg.GetComponent<Renderer>().sharedMaterial = gold;
        eggs.Add(egg);
    }

    private GameObject CreatePrimitive(string name, PrimitiveType type, Vector3 position, Vector3 scale, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(type);
        obj.name = name;
        obj.transform.position = position;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = material;
        return obj;
    }

    private void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Ranch camera");
        gameCamera = cameraObject.AddComponent<Camera>();
        gameCamera.fieldOfView = 48f;
        gameCamera.nearClipPlane = 0.1f;
        gameCamera.farClipPlane = 80f;
        gameCamera.transform.position = player.transform.position + new Vector3(0f, 10f, -11f);
        gameCamera.transform.LookAt(player.transform.position + Vector3.up * 0.5f);
        cameraObject.AddComponent<AudioListener>();
    }

    private void HandleWorldTap()
    {
        if (!Input.GetMouseButtonDown(0) || IsUiTouch(Input.mousePosition))
        {
            return;
        }

        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            ChickenActor chicken = hit.collider.GetComponentInParent<ChickenActor>();
            if (chicken != null && !chicken.IsPlayer)
            {
                player.MoveTo(chicken.transform.position + new Vector3(1f, 0f, -0.5f));
                status = "Đã tìm thấy " + chicken.DisplayName + ". Chọn VUỐT VE hoặc CHO ĂN!";
            }
            else
            {
                player.MoveTo(hit.point);
            }
        }
    }

    private bool IsUiTouch(Vector3 screenPosition)
    {
        float y = Screen.height - screenPosition.y;
        float scale = Mathf.Clamp(Screen.width / 900f, 0.75f, 1.5f);
        return y < 140f * scale || y > Screen.height - 185f;
    }

    private ChickenActor NearestFriend()
    {
        ChickenActor nearest = null;
        float nearestDistance = 3.2f;
        foreach (ChickenActor chicken in chickens)
        {
            if (chicken == player || chicken == null)
            {
                continue;
            }
            float distance = Vector3.Distance(player.transform.position, chicken.transform.position);
            if (distance < nearestDistance)
            {
                nearest = chicken;
                nearestDistance = distance;
            }
        }
        return nearest;
    }

    private FarmPlot NearestPlot()
    {
        FarmPlot nearest = null;
        float nearestDistance = 3.5f;
        foreach (FarmPlot plot in plots)
        {
            float distance = Vector3.Distance(player.transform.position, plot.transform.position);
            if (distance < nearestDistance)
            {
                nearest = plot;
                nearestDistance = distance;
            }
        }
        return nearest;
    }

    private void PetChicken()
    {
        ChickenActor friend = NearestFriend();
        if (friend == null)
        {
            SetStatus("Hãy đến gần một chú gà để vuốt ve nhé.");
            return;
        }
        friend.Pet();
        SetStatus(friend.DisplayName + " vui vẻ hơn rồi! Hạnh phúc: " + Mathf.RoundToInt(friend.Happiness) + "%");
    }

    private void FeedChicken()
    {
        ChickenActor friend = NearestFriend();
        if (friend == null)
        {
            SetStatus("Hãy đến gần gà trước khi cho ăn.");
            return;
        }
        if (grain < 1)
        {
            SetStatus("Hết thức ăn! Thu hoạch cây trồng để lấy thêm ngũ cốc.");
            return;
        }
        grain--;
        friend.Feed();
        SetStatus(friend.DisplayName + " đã ăn ngon lành. Còn " + grain + " phần thức ăn.");
    }

    private void PlantOrHarvest()
    {
        FarmPlot plot = NearestPlot();
        if (plot == null)
        {
            SetStatus("Hãy đến gần luống đất ở phía trước nông trại.");
            return;
        }
        if (plot.IsReady)
        {
            plot.Harvest();
            grain += 2;
            coins += 5;
            SetStatus("Thu hoạch thành công! +2 ngũ cốc, +5 xu.");
            return;
        }
        if (plot.IsPlanted)
        {
            SetStatus("Cây đang lớn. Quay lại sau một lát nhé!");
            return;
        }
        if (seeds < 1)
        {
            SetStatus("Hết hạt giống. Hãy ghé xưởng thức ăn hoặc hoàn thành nhiệm vụ.");
            return;
        }
        seeds--;
        plot.Plant();
        SetStatus("Đã gieo hạt! Cây sẽ sẵn sàng sau khoảng 35 giây.");
    }

    private void CollectEggs()
    {
        int collected = 0;
        for (int i = eggs.Count - 1; i >= 0; i--)
        {
            if (eggs[i] == null)
            {
                eggs.RemoveAt(i);
                continue;
            }
            if (Vector3.Distance(player.transform.position, eggs[i].transform.position) <= 3.5f)
            {
                Destroy(eggs[i]);
                eggs.RemoveAt(i);
                collected++;
            }
        }
        if (collected > 0)
        {
            coins += collected * 8;
            SetStatus("Trứng tươi ngon! +" + (collected * 8) + " xu.");
        }
        else
        {
            SetStatus("Chưa có trứng ở gần. Hãy ghé chuồng gà!");
        }
    }

    private void SetStatus(string message)
    {
        status = message;
    }

    private void OnGUI()
    {
        EnsureStyles();
        float scale = Mathf.Clamp(Screen.width / 900f, 0.75f, 1.5f);
        float margin = 16f * scale;
        float headerHeight = 68f * scale;
        GUI.Box(new Rect(margin, margin, Mathf.Min(Screen.width - 2f * margin, 540f * scale), headerHeight),
            GUIContent.none, panelStyle);
        GUI.Label(new Rect(margin + 14f * scale, margin + 7f * scale, 270f * scale, 28f * scale),
            "HAPPY HEN RANCH", labelStyle);
        GUI.Label(new Rect(margin + 14f * scale, margin + 35f * scale, 520f * scale, 24f * scale),
            "Xu  " + coins + "     Ngũ cốc  " + grain + "     Hạt giống  " + seeds + "     Trứng  " + eggs.Count,
            smallStyle);

        float messageWidth = Mathf.Min(Screen.width - 2f * margin, 500f * scale);
        float messageY = margin + headerHeight + 6f * scale;
        GUI.Box(new Rect(margin, messageY, messageWidth, 40f * scale),
            GUIContent.none, panelStyle);
        GUI.Label(new Rect(margin + 10f * scale, messageY + 5f * scale,
            messageWidth - 20f * scale, 30f * scale), status, smallStyle);

        float buttonHeight = 54f * scale;
        float buttonWidth = Mathf.Min(112f * scale, Screen.width * 0.2f);
        float bottom = Screen.height - buttonHeight - margin;
        float dpadX = margin;
        float dpadY = bottom - buttonHeight - 8f * scale;
        if (GUI.Button(new Rect(dpadX + buttonWidth + 6f * scale, dpadY, buttonWidth, buttonHeight), "▲", buttonStyle))
            StepPlayer(Vector3.forward);
        if (GUI.Button(new Rect(dpadX, dpadY + buttonHeight + 5f * scale, buttonWidth, buttonHeight), "◀", buttonStyle))
            StepPlayer(Vector3.left);
        if (GUI.Button(new Rect(dpadX + buttonWidth + 6f * scale, dpadY + buttonHeight + 5f * scale, buttonWidth, buttonHeight), "▼", buttonStyle))
            StepPlayer(Vector3.back);
        if (GUI.Button(new Rect(dpadX + (buttonWidth + 6f * scale) * 2f, dpadY + buttonHeight + 5f * scale, buttonWidth, buttonHeight), "▶", buttonStyle))
            StepPlayer(Vector3.right);

        float actionsX = Screen.width - margin - buttonWidth;
        if (GUI.Button(new Rect(actionsX, bottom, buttonWidth, buttonHeight), "TRỨNG", buttonStyle))
            CollectEggs();
        if (GUI.Button(new Rect(actionsX - buttonWidth - 6f * scale, bottom, buttonWidth, buttonHeight), "GIEO / GẶT", buttonStyle))
            PlantOrHarvest();
        if (GUI.Button(new Rect(actionsX, bottom - buttonHeight - 7f * scale, buttonWidth, buttonHeight), "CHO ĂN", buttonStyle))
            FeedChicken();
        if (GUI.Button(new Rect(actionsX - buttonWidth - 6f * scale, bottom - buttonHeight - 7f * scale, buttonWidth, buttonHeight), "VUỐT VE", buttonStyle))
            PetChicken();

    }

    private void StepPlayer(Vector3 direction)
    {
        player.MoveTo(player.transform.position + direction * 2.2f);
    }

    private void EnsureStyles()
    {
        if (stylesReady)
        {
            return;
        }
        panelTexture = MakeTexture(new Color(0.12f, 0.22f, 0.18f, 0.9f));
        buttonTexture = MakeTexture(new Color(0.95f, 0.65f, 0.24f, 0.96f));
        panelStyle = new GUIStyle(GUI.skin.box);
        panelStyle.normal.background = panelTexture;
        panelStyle.border = new RectOffset(14, 14, 14, 14);
        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = Mathf.RoundToInt(19f * Mathf.Clamp(Screen.width / 900f, 0.75f, 1.5f));
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;
        smallStyle = new GUIStyle(labelStyle);
        smallStyle.fontSize = Mathf.RoundToInt(12f * Mathf.Clamp(Screen.width / 900f, 0.75f, 1.5f));
        smallStyle.fontStyle = FontStyle.Normal;
        smallStyle.wordWrap = true;
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.background = buttonTexture;
        buttonStyle.hover.background = buttonTexture;
        buttonStyle.active.background = buttonTexture;
        buttonStyle.normal.textColor = new Color(0.18f, 0.15f, 0.1f);
        buttonStyle.fontSize = Mathf.RoundToInt(13f * Mathf.Clamp(Screen.width / 900f, 0.75f, 1.5f));
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.border = new RectOffset(12, 12, 12, 12);
        stylesReady = true;
    }

    private Texture2D MakeTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private void OnDestroy()
    {
        if (panelTexture != null) Destroy(panelTexture);
        if (buttonTexture != null) Destroy(buttonTexture);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Messenger : MonoBehaviour
{
    public AppLayer appLayer;

    private RectTransform chatContent;
    private ScrollRect scrollRect;
    private TextMeshProUGUI contactTitle;
    private GameObject choicePanel;
    private TextMeshProUGUI leftChoiceText, rightChoiceText;

    private readonly string[] chatList = {
        "Избранное", "Макс", "Саня", "Мама", "Олег", "Sushi&Pizza", "Ольга Владимировна", "Атлас/main", "Дмитрий Анатольевич"
    };

    private Dictionary<string, List<MessageData>> chatHistory = new Dictionary<string, List<MessageData>>();
    private List<GameObject> btnList = new List<GameObject>();
    private string activeChat = "";
    private bool maxDialogueStarted = false;
    public bool typing = false;

    public TMP_FontAsset font;

    private class MessageData
    {
        public string sender;
        public string text;
        public string time;
        public bool isPlayer;
    }

    void Start()
    {
        InitializeData();

        if (appLayer != null)
        {
            appLayer.sizeDelta = new Vector2(600, 450);
            appLayer.appMinSize = new Vector2(600, 450);
            appLayer.color = new Color(0.21f, 0.21f, 0.21f);
        }

        BuildUI();
        //SelectChat("Макс"); 
    }

    void InitializeData()
    {
        chatHistory.Clear();
        foreach (string contact in chatList)
        {
            chatHistory.Add(contact, new List<MessageData>());
        }
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Егор",
            text = "Почти готово",
            time = "",
            isPlayer = true
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Макс",
            text = "Я графики обновил",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Егор",
            text = "Видел",
            time = "",
            isPlayer = true
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Макс",
            text = "Ну и?",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Егор",
            text = "Я их переделал",
            time = "",
            isPlayer = true
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Макс",
            text = "Серьезно?",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Егор",
            text = "Да",
            time = "",
            isPlayer = true
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Макс",
            text = "Можно было хотя бы сказать",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Егор",
            text = "Говорю сейчас",
            time = "",
            isPlayer = true
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Макс",
            text = "У тебя все не так, если не ты сделал",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Егор",
            text = "Если бы я оставил как есть, мы бы это не сдали",
            time = "",
            isPlayer = true
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Макс",
            text = "Нормально там все было",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Егор",
            text = "Нет",
            time = "",
            isPlayer = true
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Так, спокойно.",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Сначала заканчиваем.\r\n",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Егор",
            text = "Я вообще-то тоже работаю",
            time = "",
            isPlayer = true
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Макс",
            text = "Угу",
            time = ""
        });
        chatHistory["Атлас/main"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "К утру мне нужен результат.",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "",
            text = "Данные повреждены",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Егор, как продвигается Атлас?",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Егор",
            text = "Почти готово",
            time = "",
            isPlayer = true
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Хорошо. По срокам все помнишь?",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Егор",
            text = "Да",
            time = "",
            isPlayer = true
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Макс подключился?",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Егор",
            text = "Формально да",
            time = "",
            isPlayer = true
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Это как понимать?",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Егор",
            text = "Примерно как звучит",
            time = "",
            isPlayer = true
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Егор, без этого.",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Егор",
            text = "Хорошо",
            time = "",
            isPlayer = true
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Мне нужен результат. Остальное потом.",
            time = ""
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "",
            text = "CRC mismatch / sector 914",
            time = ""
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "По оплате. Никаких самостоятельных решений.",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Егор",
            text = "В смысле",
            time = "",
            isPlayer = true
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "В прямом. Сначала сдача, потом распределение.",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Егор",
            text = "А что я есть буду?",
            time = "",
            isPlayer = true
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Сейчас не об этом.",
            time = ""
        });


        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Егор",
            text = "Именно об этом",
            time = "",
            isPlayer = true
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Егор.",
            time = ""
        });

        chatHistory["Дмитрий Анатольевич"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Не надо усложнять.",
            time = ""
        });



        chatHistory["Дмитрий"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Я долго не решался написать.",
            time = "12:05"
        });
        chatHistory["Дмитрий"].Add(new MessageData
        {
            sender = "Егор",
            text = "Ты о чем?",
            time = "12:06",
            isPlayer = true
        });
        chatHistory["Дмитрий"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Я делал вид что у нас дружная команда, я просто не хотел видеть другого. Я старался не обращать внимание на то, что ты один тащишь вашу с Максом часть. А Макс много прогуливает и залетов тоже достаточно. Ему зарплату очень сильно придерживали. И за эти году там скопилась приличная сумма. Я хотел ее оставить себе, но вчера увидел тебя одного в офисе, и во мне что-то перемкнуло.",
            time = "12:07"
        });
        chatHistory["Дмитрий"].Add(new MessageData
        {
            sender = "Дмитрий",
            text = "Я не самый добрый, справедливый, правильный человек, но здесь я хочу поступить честно. Эти деньги твои.",
            time = "12:07"
        });
        chatHistory["Дмитрий"].Add(new MessageData
        {
            sender = "Егор",
            text = "Не ожидал. Спасибо.",
            time = "12:10",
            isPlayer = true
        });



        chatHistory["Избранное"].Add(new MessageData
        {
            sender = "Егор",
            text = "Я всегда думал, что однажды станет легче. Вот дожму релиз - станет легче. Выбью отгул - станет легче. Закрою квартал - станет легче. Вот уйду - станет легче.\n\nА потом понял одну неприятную вещь. Если внутри тебя что-то треснуло место не всегда виновато.\nИногда ты уносишь эту усталость с собой. Из офиса - домой. Из дома - в метро. Из метро - в сон. Из сна - в утро. И все что у тебя по-настоящему осталось это либо наконец признать что так больше нельзя или верить в то, что еще немного и все наладится.",
            time = "Ночь",
            isPlayer = true
        });

        chatHistory["Избранное"].Add(new MessageData
        {
            sender = "Егор",
            text = "C:\\users\\egor\\ATLAS\\hiddeen\\bank.exe\r\nlogin Eg0rdev\r\npassword b54h768!",
            time = "Ночь",
            isPlayer = true
        });





        chatHistory["Sushi&Pizza"].Add(new MessageData
        {
            sender = "System",
            text = "Заказ №442 доставлен. Сумма: 1240р.",
            time = "Вчера"
        });
    }

    void BuildUI()
    {
        // 1. Главный горизонтальный контейнер
        GameObject main = new GameObject("Main", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        main.transform.SetParent(transform, false);
        Stretch(main.GetComponent<RectTransform>());
        var hGroup = main.GetComponent<HorizontalLayoutGroup>();
        hGroup.childControlWidth = true; hGroup.childControlHeight = true; hGroup.childForceExpandWidth = false;

        // 2. Sidebar (150 шириной)
        GameObject sideBar = new GameObject("Sidebar", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup));
        sideBar.transform.SetParent(main.transform, false);
        sideBar.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
        sideBar.AddComponent<LayoutElement>().flexibleWidth = .5f;
        var vGroup = sideBar.GetComponent<VerticalLayoutGroup>();
        vGroup.padding = new RectOffset(5, 5, 5, 5); vGroup.spacing = 5;
        vGroup.childControlWidth = true; vGroup.childForceExpandHeight = false;

        foreach (var name in chatList) CreateContactButton(name, sideBar.transform);

        // 3. Chat Area (Правая часть)
        GameObject chatArea = new GameObject("ChatArea", typeof(RectTransform), typeof(VerticalLayoutGroup));
        chatArea.transform.SetParent(main.transform, false);
        var chatV = chatArea.GetComponent<VerticalLayoutGroup>();
        chatV.childControlWidth = true; chatV.childControlHeight = true;
        chatV.childForceExpandHeight = false; 

        // ВЕРХ: Название чата (Высота 20)
        GameObject titleGo = new GameObject("Title", typeof(RectTransform));
        titleGo.transform.SetParent(chatArea.transform, false);
        contactTitle = CreateTMPText("Contact", titleGo.transform, 14);
        contactTitle.alignment = TextAlignmentOptions.Center;
        titleGo.AddComponent<LayoutElement>().minHeight = 20;
        titleGo.AddComponent<LayoutElement>().preferredHeight = 20;

        // ЦЕНТР: Чат 
        GameObject scrollGo = new GameObject("ChatScroll", typeof(RectTransform), typeof(ScrollRect), typeof(Image));
        scrollGo.transform.SetParent(chatArea.transform, false);
        scrollGo.GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
        scrollGo.AddComponent<LayoutElement>().flexibleHeight = 1;

        scrollRect = scrollGo.GetComponent<ScrollRect>();
        scrollRect.horizontal = false;

        GameObject viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Mask), typeof(Image));
        viewport.transform.SetParent(scrollGo.transform, false);
        Stretch(viewport.GetComponent<RectTransform>());
        viewport.GetComponent<Image>().color = new Color(0.3f, 0, 0.25f, 1f);

        GameObject content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        content.transform.SetParent(viewport.transform, false);
        chatContent = content.GetComponent<RectTransform>();
        chatContent.anchorMin = new Vector2(0, 1); chatContent.anchorMax = new Vector2(1, 1); chatContent.pivot = new Vector2(0.5f, 1);
        content.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        var contentV = content.GetComponent<VerticalLayoutGroup>();
        contentV.childControlHeight = true; contentV.childForceExpandHeight = false; contentV.spacing = 10; contentV.padding = new RectOffset(55, 55, 10, 10);

        scrollRect.content = chatContent;
        scrollRect.viewport = viewport.GetComponent<RectTransform>();

        // НИЗ: Панель мыслей (Высота 60)
        choicePanel = new GameObject("ChoicePanel", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(Image));
        choicePanel.transform.SetParent(chatArea.transform, false);
        choicePanel.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.4f, 0.4f);
        var leChoice = choicePanel.AddComponent<LayoutElement>();
        leChoice.minHeight = 60; leChoice.preferredHeight = 60;

        var choiceH = choicePanel.GetComponent<HorizontalLayoutGroup>();
        choiceH.childControlWidth = true; choiceH.childForceExpandWidth = true; choiceH.spacing = 5; choiceH.padding = new RectOffset(5, 5, 5, 5);

        leftChoiceText = CreateChoiceButton("Option 1", choicePanel.transform, 1);
        rightChoiceText = CreateChoiceButton("Option 2", choicePanel.transform, 2);
        choicePanel.SetActive(false);
    }


    public void SelectChat(string name)
    {
        if (typing) return;
        if (!chatHistory.ContainsKey(name)) return;
        activeChat = name;
        contactTitle.text = name;
        choicePanel.SetActive(false);

        foreach (Transform child in chatContent) Destroy(child.gameObject);
        foreach (var msg in chatHistory[name]) SpawnMessageUI(msg);
        foreach (GameObject button in btnList) button.GetComponent<Image>().color = new Color(1f, 1, 15f, 0.1f);

        GameObject btn = GameObject.Find("Btn_"+name);
        if (btn != null)
        {
            btn.GetComponent<Image>().color = new Color(0.3f, 0 , 0.25f, 1);
        }

        if (name == "Макс" && !maxDialogueStarted) StartCoroutine(MaxSequence());
    }

    IEnumerator MaxSequence()
    {
        typing = true;
        maxDialogueStarted = true;
        yield return new WaitForSeconds(1f);
        AddMessage("Макс", "Саня, я вижу ты в сети. Наконец-то!", "18:42");
        yield return new WaitForSeconds(2f);
        AddMessage("Макс", "Ты нашел файлы?", "18:43");
        yield return new WaitForSeconds(1.5f);
        AddMessage("Макс", "Мне нужно долги отдать. Егор обещал по-честному.", "18:43");

        choicePanel.SetActive(true);
        leftChoiceText.text = "Успокойся, скоро найду";
        rightChoiceText.text = "В процессе поиска";
    }

    void AddMessage(string sender, string text, string time, bool isPlayer = false)
    {
        MessageData msg = new MessageData { sender = sender, text = text, time = time, isPlayer = isPlayer };
        chatHistory[activeChat].Add(msg);
        SpawnMessageUI(msg);
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    void SpawnMessageUI(MessageData data)
    {
        GameObject msgGo = new GameObject("Msg", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(Image));
        msgGo.transform.SetParent(chatContent, false);
        msgGo.GetComponent<Image>().color = data.isPlayer ? new Color(0.5f, 0.5f, 0.5f, 0.4f) : new Color(0.3f, 0.3f, 0.3f, 0.4f);
        var txt = CreateTMPText($"[{data.time}] {data.sender}: {data.text}", msgGo.transform, 14);
        txt.alignment = data.isPlayer ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
    }

    void CreateContactButton(string name, Transform parent)
    {
        GameObject btnGo = new GameObject("Btn_" + name, typeof(RectTransform), typeof(Button), typeof(Image));
        btnGo.transform.SetParent(parent, false);
        btnGo.AddComponent<LayoutElement>().minHeight = 35;
        btnGo.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        var t = CreateTMPText(name, btnGo.transform, 13);
        t.alignment = TextAlignmentOptions.Center;
        btnGo.GetComponent<Button>().onClick.AddListener(() => SelectChat(name));
        btnList.Add(btnGo);
    }

    TextMeshProUGUI CreateChoiceButton(string label, Transform parent, int id)
    {
        GameObject btnGo = new GameObject("Choice_" + id, typeof(RectTransform), typeof(Button), typeof(Image));
        btnGo.transform.SetParent(parent, false);
        btnGo.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        var t = CreateTMPText(label, btnGo.transform, 13);
        t.alignment = TextAlignmentOptions.Center;
        btnGo.GetComponent<Button>().onClick.AddListener(() => {
            if (id == 1) AddMessage("Егор", leftChoiceText.text, "18:44", true);
            else AddMessage("Егор", rightChoiceText.text, "18:44", true);
            choicePanel.SetActive(false);
            typing = false;
        });
        return t;
    }

    TextMeshProUGUI CreateTMPText(string content, Transform parent, int size)
    {
        GameObject go = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        Stretch(go.GetComponent<RectTransform>());
        var t = go.GetComponent<TextMeshProUGUI>();
        t.text = content; t.fontSize = size; t.color = Color.white;
        t.font = font;
        return t;
    }

    void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
    }
}
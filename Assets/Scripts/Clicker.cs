using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Clicker : MonoBehaviour
{
    public static Clicker instance;

    public static int clickValue = 1;

    public ParticleSystem clickEffect;

    [Header("UI")]
    public TextMeshProUGUI clickText;
    public TextMeshProUGUI cpsText;

    [Header("Audio")]
    public AudioClip clickSound;

    private AudioSource audioSource;


    public long Clicks
    {
        get { return clicks; }
        set
        {
            clicks = value;

            var sequence = DOTween.Sequence();
            clickEffect.Play();
            audioSource.PlayOneShot(clickSound);
            //sequence.Append(transform.DOScale(Vector3.one, 0.1f).ChangeStartValue(Vector3.one * 1.4f));
            sequence.Append(clickText.transform.DOScale(Vector3.one, 0.1f).ChangeStartValue(Vector3.zero).OnComplete(() => clickText.text = FormatNumber(clicks)));
            sequence.Play();
        }
    }

    private long clicks = 0;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(CountClicksPerSecond());
        Load();
    }


    private void OnMouseDown()
    {
        Clicks += clickValue;
    }


    private string FormatNumber(long number)
    {
        if (number >= 1_000_000_000_000)
        {
            return (number / 1_000_000_000_000f).ToString("0.##") + "T"; // Trillion
        }
        else if (number >= 1_000_000_000)
        {
            return (number / 1_000_000_000f).ToString("0.##") + "B"; // Billion
        }
        else if (number >= 1_000_000)
        {
            return (number / 1_000_000f).ToString("0.##") + "M"; // Million
        }
        else if (number >= 1_000)
        {
            return (number / 1_000f).ToString("0.##") + "K"; // Thousand
        }
        else
        {
            return number.ToString();
        }
    }


    private IEnumerator CountClicksPerSecond()
    {
        while (true)
        {
            long previousClicks = clicks;
            yield return new WaitForSeconds(1f);
            cpsText.text = FormatNumber(clicks - previousClicks) + " cps";
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }


    private void OnApplicationQuit()
    {
        Save();
    }


    void Load()
    {
        if (PlayerPrefs.HasKey("Clicks"))
        {
            Clicks = PlayerPrefs.GetInt("Clicks");
        }
    }


    void Save()
    {
        PlayerPrefs.SetInt("Clicks", (int)Clicks);
    }
}

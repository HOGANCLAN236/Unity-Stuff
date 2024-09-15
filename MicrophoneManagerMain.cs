using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MicrophoneManagerMain : MonoBehaviour
{
    public MicrophoneManager microphoneManager;
    TMP_Dropdown DD;
    public Slider Volume;
    public float Treshold;
    public Image sliderImg;
    string[] res;
    string Option;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        res = Microphone.devices;
        DD.ClearOptions();
        List<string> Res = new List<string>();

        for (int i = 0; i < res.Length; i++)
        {
            Option = res[i];
            Res.Add(Option);
        }

        DD.AddOptions(Res);
        DD.RefreshShownValue();
    }

    float testSound;
    static float MicLoudness;
    string _device;
    AudioClip _clipRecord;
    int _sampleWindow = 128;
    bool _isInitialized;
    

    public virtual void InitMic()
    {
        if (_device == null)
        {
            _device = Option;
            _clipRecord = Microphone.Start(_device, true, 999, 44100);
        }

    }

    public virtual void StopMicrophone()
    {
        Microphone.End(_device);
    }

    public virtual float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(_device) - (_sampleWindow + 1);
        if (micPosition < 0)
        {
            return 0;
        }
        _clipRecord.GetData(waveData, micPosition);
        for (int i = 0; i < _sampleWindow; ++i)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    public virtual void Update()
    {
        if (microphoneManager == null)
        {
            microphoneManager = FindAnyObjectByType<MicrophoneManager>();
        }
        else
        {
            DD = microphoneManager.DD;
            Treshold = microphoneManager.Threshold.value;
        }

        MicLoudness = LevelMax();
        testSound = MicLoudness;

        float ThresholdAm = Treshold * 2;

        MicLoudness = MicLoudness / ThresholdAm;

        Volume.value = MicLoudness;
        if (Volume.value > 0.65)
        {
            sliderImg.color = Color.red;
        }
        else
        {
            sliderImg.color = Color.grey;
        }
    }

    public virtual void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    public virtual void OnDisable()
    {
        StopMicrophone();
    }

    public virtual void OnDestory()
    {
        StopMicrophone();
    }

    public virtual void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (!_isInitialized)
            {
                InitMic();
                _isInitialized = true;
            }
        }

        if (!focus)
        {
            StopMicrophone();
            _isInitialized = false;
        }
    }

}


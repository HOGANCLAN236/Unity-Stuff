using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;


public class MicrophoneManager : MonoBehaviour
{
    public TMP_Dropdown DD;
    public Slider Volume;
    public Slider Threshold;
    public Image SliderImg;
    string[] res;
    string Option;

    private void Start()
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

    public virtual void OnEnable()
    {
        InitMic();
        _isInitialized = true;
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
            Debug.Log(_clipRecord);
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
        MicLoudness = LevelMax();
        testSound = MicLoudness;

        float ThresholdAm = Threshold.value * 2;

        MicLoudness = MicLoudness / ThresholdAm;

        Volume.value = MicLoudness;
        if (Volume.value > 0.65)
        {
            SliderImg.color = Color.red;
        }
        else
        {
            SliderImg.color = Color.grey;
        }
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

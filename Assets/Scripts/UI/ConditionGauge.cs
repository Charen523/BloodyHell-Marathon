using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ConditionGauge : MonoBehaviourPunCallbacks
{
  [SerializeField] private GameObject _object;
  [SerializeField] private ConditionType _type;
  [SerializeField] private Image _meterImg;
  [SerializeField] private Color _color;

  public float _fillSpeed = 5f;
  private Coroutine _lerpCoroutine;
  private ICondition _condition;

  private void Awake()
  {
    _meterImg.color = _color;
    ICondition[] conditions = _object.GetComponents<ICondition>();
    foreach (ICondition condition in conditions)
    {
      if (condition.Type == _type)
      {
        _condition = condition;
        break;
      }
    }

    if (_condition != null)
    {
      _condition.OnModifyEvent += UpdateUI;
    }
  }

  private void UpdateUI(int current, int max)
  {

    // if (_lerpCoroutine != null)
    //   StopCoroutine(_lerpCoroutine);

    // _lerpCoroutine = StartCoroutine(LerpFillAmount((float)current / max));
    photonView.RPC(nameof(UpdateUIRPC), RpcTarget.All, current, max);
  }

  [PunRPC]
  public void UpdateUIRPC(int current, int max)
  {
    if (_lerpCoroutine != null)
      StopCoroutine(_lerpCoroutine);

    _lerpCoroutine = StartCoroutine(LerpFillAmount((float)current / max));
  }

  private IEnumerator LerpFillAmount(float targetFillAmount)
  {
    while (!Mathf.Approximately(_meterImg.fillAmount, targetFillAmount))
    {
      _meterImg.fillAmount = Mathf.Lerp(_meterImg.fillAmount, targetFillAmount, Time.deltaTime * _fillSpeed);
      yield return null;
    }
  }
}
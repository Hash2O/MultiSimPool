using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePage : MonoBehaviour
{
   [SerializeField] 
   private TMPro.TMP_Text _text;


   private IEnumerator Start()
   {
      while (PlayerManager.Instance == null)
      {
         yield return null;
      }
      PlayerManager.Instance.OnPlayerTurnChanged.AddListener(OnPlayerTurnChanged);
   }

   private void OnPlayerTurnChanged(Player newPlayer)
   {
      if (newPlayer.IsLocalPlayer())
      {
         _text.text = $"{newPlayer.PlayerName}, it's your turn!";
      }
      else
      {
         _text.text = $"{newPlayer.PlayerName} turn";         
      }

   }
}

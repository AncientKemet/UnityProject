using UnityEngine;
using System.Collections;

public class UnitFactory : Monosingleton<UnitFactory> {

	[SerializeField]
	private Player _playerPrefab;

	private Player CreatePlayer(int id){
		Player _player = ((GameObject)Instantiate (_playerPrefab.gameObject)).GetComponent<Player>();
		_player.ID = id;
	}

}

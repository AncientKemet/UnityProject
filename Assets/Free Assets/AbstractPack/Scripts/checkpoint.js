/* http://formation-facile.fr (c)

Ce script permet de vérifier les colisions entre l'avion et les checkpoints afin d'augmenter le score
Check if the plane touch a checkpoint and add 10 points
*/

var chk : AudioClip;

function OnTriggerEnter(objetInfo : Collider) {
	
	if (objetInfo.gameObject.tag == "chk")
		{
		audio.PlayOneShot(chk);
	cam_score.score += 10;
		}
		
		}
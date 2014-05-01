/* http://formation-facile.fr (c)

Ce script permet d'ajouter un système de score
This script permit ti add a score system to your game
*/

static var score : int = 0;   

function OnGUI() {
    GUI.color = Color.yellow;
    GUI.TextArea(Rect(15,15,100,25),' Score : '+ score );
						}
function Update(){
if (score <= 0){		score = 0;		}
if (score >= 999){		score = 999;		}
						}


/* http://formation-facile.fr (c)

Ce script permet de faire voler l'avion et de le controler
This script make the plane fly and allow you to control it
*/

var vitesse = 15;
var vitesseRotation = 1.0;

function Update () {
transform.Translate(Vector3.forward * Time.deltaTime * vitesse);

var controller : CharacterController = GetComponent(CharacterController);
transform.Rotate(0, Input.GetAxis("Horizontal") * vitesseRotation, 0);

transform.Rotate(Input.GetAxis("Vertical") * vitesseRotation, 0, 0);

if (transform.rotation.z < -3)
transform.rotation.z = -3;
if (transform.rotation.z > 3)
transform.rotation.z = 3;

}

@script RequireComponent (CharacterController)
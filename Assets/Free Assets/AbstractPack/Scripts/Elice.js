/* http://formation-facile.fr (c)

Ce script permet de faire tourner l'�lice de l'avion
Anim the plane
*/

var rotationSpeed : float = 6.0;
function Update () {
transform.Rotate(Vector3(rotationSpeed, 0, 0));
}
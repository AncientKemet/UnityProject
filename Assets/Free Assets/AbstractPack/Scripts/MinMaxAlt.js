/* http://formation-facile.fr (c)

Altitude
*/

function Update () {
if (transform.position.y < 1)
transform.position.y = 1;

if (transform.position.y > 100)
transform.position.y = 100;
}
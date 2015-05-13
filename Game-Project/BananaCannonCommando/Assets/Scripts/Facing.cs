using UnityEngine;
using System.Collections;

public class Facing : MonoBehaviour {

	private float rad = 45f * Mathf.Deg2Rad;

	public int DetermineSprite (Vector3 facing)
	{
		// Sprites are ordered from 0 to 7 as follows:
		// 0 - along the game objects forward vector
		// 1 - 45 degrees to the right of the forward vector
		// 2 - along the game objects right vector
		// 3 - 45 degrees to the right of the right vector
		// 4 - along the game objects -forward vector
		// 5 - 45 degrees to the right of the -forward vector
		// 6 - along the game objects -right vector
		// 7 - 45 degrees to the right of the -right vector
		Vector3 normFace = facing.normalized;

		Vector3 facingDirection_1, facingDirection_2, facingDirection_3, facingDirection_4,
		facingDirection_5, facingDirection_6, facingDirection_7, facingDirection_0;

		facingDirection_0 = transform.forward;
		facingDirection_1 = Vector3.RotateTowards (transform.forward, transform.right, rad, 0.0F);
		facingDirection_2 = transform.right;
		facingDirection_3 = Vector3.RotateTowards (-transform.forward, transform.right, rad, 0.5F);
		facingDirection_4 = -transform.forward;
		facingDirection_5 = Vector3.RotateTowards (-transform.forward, -transform.right, rad, 0.0F);
		facingDirection_6 = -transform.right;
		facingDirection_7 = Vector3.RotateTowards (transform.forward, -transform.right, rad, 0.0F);

		int spriteNum = 0;

		// if the x value is positive
		if ((0f < normFace.x) && (normFace.x <= 1f)) {
			// if the z value is positive
			if ((0f < normFace.z) && (normFace.z <= 1f)) {
				// we are going to be using directions between forward (0) and right (2)
				// determine which of the three vectors is closer to the facing
				float angle_0, angle_1, angle_2;

				angle_0 = Vector3.Angle (normFace, facingDirection_0);
				angle_1 = Vector3.Angle (normFace, facingDirection_1);
				angle_2 = Vector3.Angle (normFace, facingDirection_2);

				// if the facing is closer to forward than it is to right
				if (angle_0 <= angle_2) {

					// and if facing is closer to forward than the halfway
					if (angle_0 < angle_1) {

						// the sprite is facing the forward vector
						spriteNum = 0;
					} else {
						// otherwise the sprite is facing the halfway vector
						spriteNum = 1;
					}
					// if facing is closer to right than it is to forward
					// and if facing is closer to right than the halfway
				} else if (angle_2 < angle_1) {
					// the sprite is facing the right vector
					spriteNum = 2;
				} else {
					// otherwise the sprite is facing the halfway vector
					spriteNum = 1;
				}
				// if the z value is negative
			} else {
				// we are going to be using directions between right (2) and -forward (4)
				// determine which of the three vectors is closer to the facing
				float angle_0, angle_1, angle_2;
				
				angle_0 = Vector3.Angle (normFace, facingDirection_2);
				angle_1 = Vector3.Angle (normFace, facingDirection_3);
				angle_2 = Vector3.Angle (normFace, facingDirection_4);

				// if facing is closer to right than -forward
				if (angle_0 <= angle_2) {
					// if facing is closer to right than halfway
					if (angle_0 < angle_1) {
						// the sprite is facing the right vector
						spriteNum = 2;
					} else {
						// otherwise the sprite is facing the halfway vector
						spriteNum = 3;
					}
					// if facing is closer to -forward
					// and if facing is closer to -forward than halfway
				} else if (angle_2 < angle_1) {
					// the sprite is facing -forward
					spriteNum = 4;
				} else {
					// otherwise the sprite is facing the halfway vector
					spriteNum = 3;
				}
			}
			// if the x value is negative
		} else {
			// if the z value is positive
			if ((0f < normFace.z) && (normFace.z <= 1f)) {
				// we are going to be using directions between -right (6) and right (0)
				// determine which of the three vectors is closer to the facing
				float angle_0, angle_1, angle_2;
				
				angle_0 = Vector3.Angle (normFace, facingDirection_6);
				angle_1 = Vector3.Angle (normFace, facingDirection_7);
				angle_2 = Vector3.Angle (normFace, facingDirection_0);
				
				// if the facing is closer to -right than it is to forward
				if (angle_0 <= angle_2) {
					
					// and if facing is closer to -right than the halfway
					if (angle_0 < angle_1) {
						
						// the sprite is facing the -right vector
						spriteNum = 6;
					} else {
						// otherwise the sprite is facing the halfway vector
						spriteNum = 7;
					}
					// if facing is closer to forward than it is to -right
					// and if facing is closer to forward than the halfway
				} else if (angle_2 < angle_1) {
					// the sprite is facing the forward vector
					spriteNum = 0;
				} else {
					// otherwise the sprite is facing the halfway vector
					spriteNum = 7;
				}
				// if the z value is negative
			} else {
				// we are going to be using directions between -forward (4) and -right (6)
				// determine which of the three vectors is closer to the facing
				float angle_0, angle_1, angle_2;
				
				angle_0 = Vector3.Angle (normFace, facingDirection_4);
				angle_1 = Vector3.Angle (normFace, facingDirection_5);
				angle_2 = Vector3.Angle (normFace, facingDirection_6);
				
				// if facing is closer to -forward than -right
				if (angle_0 <= angle_2) {
					// if facing is closer to -forward than halfway
					if (angle_0 < angle_1) {
						// the sprite is facing the -forward vector
						spriteNum = 4;
					} else {
						// otherwise the sprite is facing the halfway vector
						spriteNum = 5;
					}
					// if facing is closer to -right
					// and if facing is closer to -right than halfway
				} else if (angle_2 < angle_1) {
					// the sprite is facing -forward
					spriteNum = 6;
				} else {
					// otherwise the sprite is facing the halfway vector
					spriteNum = 5;
				}
			}
		}

		return spriteNum;
	}
}

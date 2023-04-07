using Godot;

public partial class Enemy : RigidBody2D
{
	public override void _Ready() {
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] enemyTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(enemyTypes[GD.Randi() % enemyTypes.Length]);
	}
	
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		QueueFree();
	}
}

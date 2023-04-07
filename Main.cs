using Godot;

public partial class Main : Node
{
	[Export]
	public PackedScene EnemyScene { get; set; }
	
	private int _score;
	
	public override void _Ready() {
		//NewGame();
	}
	
	public void GameOver()
	{
		GetNode<Timer>("EnemyTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		
		GetNode<HUD>("HUD").ShowGameOver();
		
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
	}
	
	public void NewGame() {
		_score = 0;
		
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		GetTree().CallGroup("enemies", Node.MethodName.QueueFree);
		
		GetNode<Timer>("StartTimer").Start();
		
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		
		GetNode<AudioStreamPlayer>("Music").Play();
	}
	
	private void _on_enemy_timer_timeout()
	{
		Enemy enemy = EnemyScene.Instantiate<Enemy>();
		
		PathFollow2D enemySpawnLocation = GetNode<PathFollow2D>("EnemyPath/EnemySpawnLocation");
		enemySpawnLocation.ProgressRatio = GD.Randf();
		
		float direction = enemySpawnLocation.Rotation + Mathf.Pi / 2;
		
		enemy.Position = enemySpawnLocation.Position;
		
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		enemy.Rotation = direction;
		
		Vector2 velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		enemy.LinearVelocity = velocity.Rotated(direction);
		
		AddChild(enemy);
	}


	private void _on_score_timer_timeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}


	private void _on_start_timer_timeout()
	{
		GetNode<Timer>("EnemyTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
}

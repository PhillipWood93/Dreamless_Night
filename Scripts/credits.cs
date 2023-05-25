using Godot;
using System;

public partial class credits : CanvasLayer
{
	private string[] _creditCategories = { "Visual Effects and Art", "Sound Effects and Music" };

	[Export] private int _categoryFontSize = 100;

	[ExportCategory("Scenes")]
	[Export] private PackedScene _creditLabelScene;

	[ExportCategory("People to Credit")]
	[Export] private string[] _vfxCredits;
	[Export] private string[] _sfxCredits;


	private bool _vfxDone = false;
	private bool _sfxDone = false;
	private bool _swapCat = true;
	private Timer _timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = (Timer)GetNode("CreditsOverTimer");

	}

	private double _time;
	private int _count;
	private bool _end = false;
	private bool _printCat = true;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_time += delta;

		if(_count >= _vfxCredits.Length && !_vfxDone)
		{
			_count = 0;
			_vfxDone=true;
		}
		if(_count >= _sfxCredits.Length && _vfxDone && !_sfxDone)
		{
			_count = 0;
			_sfxDone=true;
		}
		if (_time > 2)
		{
			if (_vfxDone && _sfxDone && !_end)
			{
				CreditLabel l = SetupLable();
				l.Text = "The End";
				_end = true;
				_timer.Start();
				return;
			}
			if (!_vfxDone) 
			{ 
				PrintVFXCredit();
			}
			if (!_sfxDone && _vfxDone) 
			{
				PrintSFXCredit();
			} 
			
			_count++;
			_time = 0;
		}
	}

	private void OnTimeout()
	{
		GetTree().ChangeSceneToFile("res://scenes/UI/start_menu.tscn");
	}

	private CreditLabel SetupLable()
	{
        CreditLabel l = (CreditLabel)_creditLabelScene.Instantiate();
        l.Position = new Vector2(152f, 651f);
		GetNode("Background").AddSibling(l);
		return l;
    }

	private void PrintCategory(int i)
	{
        CreditLabel l = (CreditLabel)_creditLabelScene.Instantiate();
        l.Position = new Vector2(152f, 651f);
        l.Text = _creditCategories[i];
		l.AddThemeFontSizeOverride("Font Size", _categoryFontSize);
		GetTree().Root.AddChild(l);
    }

	private void PrintSFXCredit()
	{
        CreditLabel l = SetupLable();
        l.Text = _sfxCredits[_count];
    }


    private void PrintVFXCredit()
	{
		CreditLabel l = SetupLable();
		l.Text = _vfxCredits[_count];
	}

}

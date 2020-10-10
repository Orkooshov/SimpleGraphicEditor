using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicalEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		enum DrawTool
		{
			None, Pencil, Line, Ellipse
		}
		DrawTool drawTool;
		SolidColorBrush color = new SolidColorBrush(Colors.Black);
		Point currentPoint = new Point();
		bool isPaint = false;

		private void button_pencil_Click(object sender, RoutedEventArgs e)
		{
			drawTool = DrawTool.Pencil;
		}

		private void button_line_Click(object sender, RoutedEventArgs e)
		{
			drawTool = DrawTool.Line;

		}

		private void button_ellipse_Click(object sender, RoutedEventArgs e)
		{
			drawTool = DrawTool.Ellipse;
		}

		private void Canvas_Main_MouseMove(object sender, MouseEventArgs e)
		{
			switch (drawTool)
			{
				case DrawTool.Pencil:
					if (!isPaint) return;
					var point = Mouse.GetPosition(Canvas_Main);
					var line = new Line
					{
						Stroke = color,
						X1 = currentPoint.X,
						Y1 = currentPoint.Y,
						X2 = point.X,
						Y2 = point.Y,
						StrokeStartLineCap = PenLineCap.Round,
						StrokeEndLineCap = PenLineCap.Round
					};
					currentPoint = point;
					Canvas_Main.Children.Add(line);
					break;
				case DrawTool.Line:

					break;
			}
		}

		private void Canvas_Main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (isPaint) return;
			isPaint = true;
			currentPoint = Mouse.GetPosition(Canvas_Main);
			var dot = new Ellipse { Fill = color };
			dot.SetValue(Canvas.LeftProperty, currentPoint.X);
			dot.SetValue(Canvas.TopProperty, currentPoint.Y);
			Canvas_Main.Children.Add(dot);
		}

		private void Canvas_Main_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			isPaint = false;
		}

		private void button_chooseColor_Click(object sender, RoutedEventArgs e)
		{
			
		}
	}
}

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace mlForm
{
    public partial class Form1 : Form
    {
        private static ILearningPipelineItem algorithm;
        List<string> _items = new List<string>();
        OpenFileDialog ofd = new OpenFileDialog();

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();

            _items.Add("StochasticDualCoordinateAscentClassifier");
            _items.Add("NaiveBayesClassifier");
            _items.Add("FastForestBinaryClassifier ");
                
            listBox1.DataSource = _items;

            textBox3.ScrollBars = ScrollBars.Both;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.SafeFileName;
            }
        }
            
        private void button2_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.SafeFileName;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                csProjectData();
                //PredictionModel<IrisData, IrisPrediction> model = Train();
                //var prediction = model.Predict(TestData.prediction);
                //textBox3.Text = $"Predicted flower type is: {prediction.PredictedLabels}";
            }
            else
            {
                MessageBox.Show("Text field cannot be empty!");
            }
        }
        public PredictionModel<IrisData, IrisPrediction> Train()
        {
            string datapath = textBox1.Text;
            int index = listBox1.SelectedIndex;
            switch (index)
            {
                case 0:
                    algorithm = new StochasticDualCoordinateAscentClassifier();
                    break;
                case 1:
                    algorithm = new NaiveBayesClassifier();
                    break;
                case 2:
                    algorithm = new FastForestBinaryClassifier();
                    break;
            }

            GetColumns gc = new GetColumns();
            var iris = gc.getColumns();

            var pipeline = new LearningPipeline {
                new TextLoader(datapath).CreateFrom<IrisData>(useHeader: true, separator: ','),
                new Dictionarizer("Label"),
                new ColumnConcatenator("Features", iris),
                algorithm,
                new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" }
            };
            PredictionModel<IrisData, IrisPrediction> model = pipeline.Train<IrisData, IrisPrediction>();
            return model;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if(textBox1.Text != ""){
                textBox3.Text = File.ReadAllText(textBox1.Text);
            }
            else
            {
                MessageBox.Show("Text field cannot be empty!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                textBox3.Text = File.ReadAllText(textBox2.Text);
            }
            else
            {
                MessageBox.Show("Text field cannot be empty!");
            }
        }

        private void csProjectData()
        {
            FileStream fs = new FileStream(@"iris-data.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string line1 = sw.ReadLine();
            string[] allLines = line1.Split(',');

            using (var file = new StreamWriter(@"code.cs", true))
            {
                file.WriteLine("using Microsoft.ML.Runtime.Api;");
                file.WriteLine();
                file.WriteLine("namespace ncML");
                file.WriteLine("{");
                file.WriteLine("public class ProjectData");
                file.WriteLine("{");
                for (int i = 0; i < allLines.Length; i++)
                {
                    file.WriteLine("[Column(\"" + i + "\")]");
                    file.WriteLine("public float " + allLines[i] + ";");
                    file.WriteLine();
                }
                file.WriteLine("}");
                file.WriteLine("public class ProjectPrediction");
                file.WriteLine("{");
                file.WriteLine("[ColumnName(\"PredictedLabel\")]");
                file.WriteLine("public string PredictedTypes;");
                file.WriteLine("}");
                file.WriteLine("}");
            }
        }
    }
}
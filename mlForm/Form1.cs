using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mlForm
{
    public partial class Form1 : Form
    {
        private static ILearningPipelineItem algorithm;
        List<string> _items = new List<string>();

        public Form1()
        {
            InitializeComponent();

            _items.Add("StochasticDualCoordinateAscentClassifier");
            _items.Add("NaiveBayesClassifier");
            _items.Add("FastForestBinaryClassifier ");

            listBox1.DataSource = _items;
        }

        OpenFileDialog ofd = new OpenFileDialog();


        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.SafeFileName;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.SafeFileName;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                PredictionModel<IrisData, IrisPrediction> model = Train();
                var prediction = model.Predict(TestData.prediction);
                textBox3.Text = $"Predicted flower type is: {prediction.PredictedLabels}";
            }
            else
            {
                MessageBox.Show("Text field cannot be empty!");
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

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

            var pipeline = new LearningPipeline {
                new TextLoader(datapath).CreateFrom<IrisData>(separator: ','),
                new Dictionarizer("Label"),
                new ColumnConcatenator("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"),
                algorithm,
                new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" }
            };
            PredictionModel<IrisData, IrisPrediction> model = pipeline.Train<IrisData, IrisPrediction>();
            return model;
        }
    }
}

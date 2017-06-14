using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using edu.stanford.nlp.hcoref;
using edu.stanford.nlp.hcoref.data;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.parser.lexparser;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.process;
using edu.stanford.nlp.sentiment;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.util;
using java.io;
using java.text;
using java.util;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Speech.Recognition;



namespace SpeechToImage
{
    public partial class Form1 : Form
    {
        Graphics g = null;
        Pen sketcher = new Pen(Color.Red);
        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10);
        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
        CoreNLP nlp = new CoreNLP();
        static int start_x, start_y;
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        Grammar start, stop;

        NAudio.Wave.WaveIn sourceStream = null;
        NAudio.Wave.DirectSoundOut waveOut = null;
        NAudio.Wave.WaveFileWriter waveWriter = null;

        public Form1()
        {
            InitializeComponent();
            nlp.InitializeNLP();
            g = canvas.CreateGraphics();
            sketcher.Width = 1;
            start_x = canvas.Width / 2;
            start_y = canvas.Height / 2;
            button4.Enabled = false;
        }
        void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("Begin"))
            {
                Console.Text += "\r\n Start recording";
                recEngine.Grammars[recEngine.Grammars.IndexOf(start)].Enabled = false;
                recEngine.Grammars[recEngine.Grammars.IndexOf(stop)].Enabled = true;
                RecognizeAudio();
                
            }
            else if (e.Result.Text.Equals("Stop"))
            {
                Console.Text += "\r\n Stopping recognition";
                recEngine.Grammars[recEngine.Grammars.IndexOf(stop)].Enabled = false;
                StopRec();
                recEngine.Grammars[recEngine.Grammars.IndexOf(start)].Enabled = true;
                
            }



        }

        void RecognizeAudio()
        {
            g.Clear(Color.White);
            Console.Text = "Say something! \n";
            ProcessedBox.Text = BasicDep.Text = AnswerBox.Text = SentiBox.Text = KeywordBox.Text = "";
            label1.Visible = true;
            panel1.Visible = true;
            button1.Enabled = false;
            /*---------------------------------------Audio wave--------------------------------*/

            List<NAudio.Wave.WaveInCapabilities> sources = new List<NAudio.Wave.WaveInCapabilities>();

            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                sources.Add(NAudio.Wave.WaveIn.GetCapabilities(i));
            }

            sourceList.Items.Clear();

            foreach (var source in sources)
            {
                ListViewItem item = new ListViewItem(source.ProductName);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, source.Channels.ToString()));
                sourceList.Items.Add(item);
            }
            sourceList.TopItem.Selected = true;

            string path = Directory.GetCurrentDirectory() + @"\question.wav";

            if (System.IO.File.Exists(path))
            {
                // Note that no lock is put on the
                // file and the possibility exists
                // that another process could do
                // something with it between
                // the calls to Exists and Delete.
                System.IO.File.Delete(path);
            }

            FileStream save = System.IO.File.Create(path);

            int deviceNumber = sourceList.SelectedItems[0].Index;

            sourceStream = new NAudio.Wave.WaveIn();
            sourceStream.DeviceNumber = deviceNumber;
            sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(deviceNumber).Channels);

            sourceStream.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(sourceStream_DataAvailable);
            waveWriter = new NAudio.Wave.WaveFileWriter(save, sourceStream.WaveFormat);

            sourceStream.StartRecording();

            //await Task.Delay(10000);
            /*---------------------------------------Audio Wave end----------------------------*/
        }

        public void DrawImage(Dictionary<string, string> props, string keyWord)
        {
            string value;
            bool change = false;
            if (keyWord == "circle")
            {
                string f = "";
                foreach (KeyValuePair<string, string> tt in props)
                {
                    if (tt.Value.Equals("Find"))
                    {
                        f = tt.Key;
                    }
                }
                
                if (props.Count == 0) Console.Text = "\r\n Please specify dimensions for better understanding";
                g.DrawLine(sketcher, new Point(start_x, start_y - 1), new Point(start_x, start_y + 1));
                g.DrawString("O", drawFont, drawBrush, start_x - 10, start_y - 20, drawFormat);
                if (props.TryGetValue("radius", out value))
                {//drawing radius if radius is a keyword
                    int rad = Int32.Parse(value);
                    if (rad < 30 && rad != 0)
                    {
                        rad += 40;
                        change = true;
                    }
                    g.DrawEllipse(sketcher, start_x - rad, start_y - rad, rad * 2, rad * 2);
                    g.DrawLine(sketcher, new Point(start_x, start_y), new Point(start_x + rad, start_y));//new Point(250, 150)
                    g.DrawString(value, drawFont, drawBrush, (start_x + ((rad - 10) / 2)), start_y - 15, drawFormat);
                    if(change)
                        rad -= 40;
                    if (f.Equals("area"))//this ain't working!!!
                    {
                       AnswerBox.Text =  Math.Round((Math.PI * Math.Pow(rad, 2)), 2 ,MidpointRounding.ToEven).ToString();
                    }
                    if (f.Equals("circumference"))
                    {
                       AnswerBox.Text  = Math.Round(((2 * Math.PI * rad)), 2 ,MidpointRounding.ToEven).ToString();
                    }
                }
                else if (props.TryGetValue("diameter", out value))// drawing diameter if it is a keyword
                {
                    int rad = Int32.Parse(value) / 2;
                    if (rad < 30 && rad != 0)
                    {
                        rad += 40;
                        change = true;
                    }
                    g.DrawEllipse(sketcher, start_x - rad, start_y - rad, rad * 2, rad * 2);
                    g.DrawLine(sketcher, new Point(start_x - rad, start_y), new Point(start_x + rad, start_y));//new Point(250, 150)
                    //g.DrawLine(sketcher, new Point(50, 150), new Point(250, 150));
                    g.DrawString(value, drawFont, drawBrush, start_x + rad / 2, start_y - 15, drawFormat);
                    if(change)
                        rad -= 40;
                    if (f.Equals("area"))//this ain't working!!!
                    {
                        AnswerBox.Text =  Math.Round((Math.PI * Math.Pow(rad, 2)),2 , MidpointRounding.ToEven).ToString();
                    }
                    if (f.Equals("circumference"))
                    {
                        AnswerBox.Text = Math.Round(((2 * Math.PI * rad)), 2 ,MidpointRounding.ToEven).ToString();
                    }
                }
                else if (props.ContainsKey("chord") || props.ContainsKey("arc") || props.ContainsKey("central") || props.ContainsKey("angle"))
                {
                    g.DrawLine(sketcher, new Point(start_x - 100, start_y), new Point(start_x, start_y));
                    g.DrawLine(sketcher, new Point(start_x, start_y + 100), new Point(start_x, start_y));
                    g.DrawLine(sketcher, new Point(start_x - 100, start_y), new Point(start_x, start_y + 100));
                    g.DrawString("C", drawFont, drawBrush, start_x - 115, start_y - 5, drawFormat);
                    g.DrawString("D", drawFont, drawBrush, start_x - 5, start_y + 105, drawFormat);
                    //g.DrawEllipse(sketcher, 50, 50, 200, 200);
                    g.DrawEllipse(sketcher, start_x - 100, start_y - 100, 200, 200);
                }
                else
                    g.DrawEllipse(sketcher, start_x - 100, start_y - 100, 200, 200);

                //drawFont.Dispose();
                //drawBrush.Dispose();
                KeywordBox.Text = keyWord;

            }

            if (keyWord == "square")
            {
                string f = "";
                foreach (KeyValuePair<string, string> tt in props)
                {
                    if (tt.Value.Equals("Find"))
                    {
                        f = tt.Key;
                    }
                }

                if (props.Count == 0) Console.Text = "\r\n Please specify dimensions for better understanding";
                if (props.TryGetValue("side", out value) || props.TryGetValue("sides", out value))
                {
                    int side = Int32.Parse(value);
                    //bool change = false;
                    if (side < 40)
                    {
                        side += 50;
                        change = true;
                    }
                    g.DrawString(value, drawFont, drawBrush, 50 + side / 2, 35, drawFormat);
                    g.DrawString(value, drawFont, drawBrush, 50 + side + 5, 50 + side / 2, drawFormat);
                    g.DrawRectangle(sketcher, 50, 50, side, side);

                    if (change)
                    {
                        side -= 50;
                        change = false;
                    }
                    if (f.Equals("area"))
                    {

                        AnswerBox.Text = (side*side).ToString();
                    }
                    if (f.Equals("perimeter") || f.Equals("perimetre"))
                    {
                        AnswerBox.Text =  (side * 4).ToString();
                    }
                }
                else if (props.TryGetValue("diagonal", out value))
                {
                    g.DrawString(value, drawFont, drawBrush, 130, 100, drawFormat);
                    g.DrawLine(sketcher, 50, 50, 250, 250);
                    g.DrawRectangle(sketcher, 50, 50, 200, 200);
                    
                    float side = (Int32.Parse(value) / (float)Math.Sqrt(2));
                    if (f.Equals("area"))
                    {
                        AnswerBox.Text = (side * side).ToString();
                    }
                    if (f.Equals("perimeter") || f.Equals("perimetre"))
                    {
                        AnswerBox.Text = (side * 4).ToString();
                    }
                }
                else
                    g.DrawRectangle(sketcher, 50, 50, 200, 200);
                KeywordBox.Text = keyWord;

            }

            if (keyWord == "rectangle")
            {

                string f = "";
                foreach (KeyValuePair<string, string> tt in props)
                {
                    if (tt.Value.Equals("Find"))
                    {
                        f = tt.Key;
                    }
                }

                if (props.Count == 0) Console.Text = "\r\n Please specify dimensions for better understanding";
                string raw_lent, raw_bre;
                if (props.TryGetValue("length", out raw_lent) && props.TryGetValue("breadth", out raw_bre))
                {
                    int len = Int32.Parse(raw_lent);
                    int bread = Int32.Parse(raw_bre);
                    //bool change = false;
                    if (len < 40 && bread <40)
                    {
                        len += 50;
                        bread += 50;
                        change = true;
                    } 

                    g.DrawRectangle(sketcher, 50, 50, len, bread);
                    g.DrawString(raw_lent, drawFont, drawBrush, 50 + len / 2, 35, drawFormat);
                    g.DrawString(raw_bre, drawFont, drawBrush, 50 + len + 5, 50 + bread / 2, drawFormat);

                    if (change)
                    {
                        len -= 50;
                        bread -= 50;
                        change = false;
                    } 

                    if (f.Equals("area"))
                    {
                        AnswerBox.Text = (len*bread).ToString();
                    }
                    if (f.Equals("perimeter") || f.Equals("perimetre"))
                    {
                        AnswerBox.Text = (2*(len+bread)).ToString();
                    }
                }
                else if (props.TryGetValue("length", out value))
                {


                    int len = Int32.Parse(value);

                    //bool change = false;
                    if (len < 40)
                    {
                        len += 50;
                        change = true;
                    } 

                    g.DrawRectangle(sketcher, 50, 50, len, len - 30);
                    g.DrawString(value, drawFont, drawBrush, 50 + len, 35, drawFormat);
                    AnswerBox.Text += "Insufficient Data";

                }
                else if (props.TryGetValue("breadth", out value))
                {
                    int bread = Int32.Parse(value);
                    //bool change = false;
                    if (bread < 40)
                    {
                        bread += 50;
                        change = true;
                    } 

                    g.DrawRectangle(sketcher, 50, 50, bread + 30, bread);
                    g.DrawString(value, drawFont, drawBrush, 50 + bread + 5, 50 + bread / 2, drawFormat);
                    AnswerBox.Text = "Insufficient Data";
                }
                else if (props.TryGetValue("diagonal", out value))
                {
                    g.DrawRectangle(sketcher, 50, 50, 220, 150);
                    g.DrawString(value, drawFont, drawBrush, 130, 100, drawFormat);
                    g.DrawLine(sketcher, 50, 50, 270, 200);
                    AnswerBox.Text = "Insufficient Data";
                }
                else
                    g.DrawRectangle(sketcher, 50, 50, 220, 150);
                KeywordBox.Text = keyWord;

            }

            if (keyWord == "triangle")
            {

                string f = "";
                foreach (KeyValuePair<string, string> tt in props)
                {
                    if (tt.Value.Equals("Find"))
                    {
                        f = tt.Key;
                    }
                }
                if (props.Count == 0) Console.Text = "\r\n Please specify dimensions for better understanding";
                Point[] points =
                 {
                    new Point(150, 50),
                    new Point(50, 250),
                    new Point(250, 250),
                    //new Point(150, 50)
                 };
                string ht, bs;
                g.DrawPolygon(sketcher, points);
                if (props.TryGetValue("height", out ht) && props.TryGetValue("base", out bs))
                {
                    g.DrawLine(sketcher, 150, 50, 150, 250);
                    g.DrawString(ht, drawFont, drawBrush, 150 + 5, 200, drawFormat);
                    g.DrawString(bs, drawFont, drawBrush, 150, 250 + 5, drawFormat);

                    if (f.Equals("area"))
                    {
                        AnswerBox.Text =  Math.Round((0.5 * Int32.Parse(ht) * Int32.Parse(bs) ) , 2 , MidpointRounding.ToEven).ToString();
                    }
                  
                }
                else if (props.TryGetValue("height", out ht))
                {
                    g.DrawLine(sketcher, 150, 50, 150, 250);
                    g.DrawString(ht, drawFont, drawBrush, 150 + 5, 200, drawFormat);
                    AnswerBox.Text = "Insufficient Data";
                }
                else if (props.TryGetValue("base", out bs))
                {
                    int bas = Int32.Parse(bs);
                    g.DrawString(bs, drawFont, drawBrush, 150, 250 + 5, drawFormat);
                    AnswerBox.Text = "Insufficient Data";
                }

                KeywordBox.Text = keyWord;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            Console.Text = "Say something! \n";
            ProcessedBox.Text = BasicDep.Text = AnswerBox.Text = SentiBox.Text = KeywordBox.Text = "";
            
            label1.Visible = true;
            panel1.Visible = true;
            button1.Enabled = false;
            /* await Task.Delay(100);
             string line = Recognition() + ".";*/
            /*---------------------------------------Audio wave--------------------------------*/

            List<NAudio.Wave.WaveInCapabilities> sources = new List<NAudio.Wave.WaveInCapabilities>();

            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                sources.Add(NAudio.Wave.WaveIn.GetCapabilities(i));
            }

            sourceList.Items.Clear();

            foreach (var source in sources)
            {
                ListViewItem item = new ListViewItem(source.ProductName);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, source.Channels.ToString()));
                sourceList.Items.Add(item);
            }
            sourceList.TopItem.Selected = true;

            string path = Directory.GetCurrentDirectory() + @"\question.wav";

            if (System.IO.File.Exists(path))
            {
                // Note that no lock is put on the
                // file and the possibility exists
                // that another process could do
                // something with it between
                // the calls to Exists and Delete.
                System.IO.File.Delete(path);
            }

            FileStream save = System.IO.File.Create(path);

            int deviceNumber = sourceList.SelectedItems[0].Index;

            sourceStream = new NAudio.Wave.WaveIn();
            sourceStream.DeviceNumber = deviceNumber;
            sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(deviceNumber).Channels);

            sourceStream.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(sourceStream_DataAvailable);
            waveWriter = new NAudio.Wave.WaveFileWriter(save, sourceStream.WaveFormat);

            sourceStream.StartRecording();

            //await Task.Delay(10000);


            /*---------------------------------------Audio Wave end----------------------------*/

        }

        private void sourceStream_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (waveWriter == null) return;

            waveWriter.WriteData(e.Buffer, 0, e.BytesRecorded);
            waveWriter.Flush();
        }

        public string Recognition()
        {
            Process p = new Process(); // create process (i.e., the python program
            p.StartInfo.FileName = "c:\\Python27\\python.exe";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false; // make sure we can read the output from stdout
            var pyfile = "\"" + Directory.GetCurrentDirectory() + "\"" + "\\wav.py";

            p.StartInfo.Arguments = pyfile; // start the python program with two parameters
            p.Start(); // start the process (the python program)
            //Thread.Sleep(100);
            StreamReader s = p.StandardOutput;
            String output = s.ReadToEnd();
            string[] ar = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            p.WaitForExit();
            string quest = RefineString(ar[0]);
            return quest;
            // write the output we got from python app 
            //Console.WriteLine("Value received from script: " + myString);
        }

        public string RefineString(string input)
        {
            string[] refiner = input.Split(' ');
            for (int i = 0; i < refiner.Length; i++ )
            {
                if (refiner[i].Equals("metre"))
                {
                    refiner[Array.IndexOf(refiner, "metre")] = "meter";
                }

                if (refiner[i].Equals("centimetre"))
                {
                    refiner[Array.IndexOf(refiner, "centimetre")] = "centimeter";
                }
                if (refiner[i].Equals("Stop") || refiner[i].Equals("stop"))
                {
                    refiner[Array.IndexOf(refiner, "stop")] = "";
                }
                if (refiner[i].Equals("square") && refiner[i + 1].Equals("centimetre"))
                {
                    refiner[Array.IndexOf(refiner, "square")] = "squarecentimeter";
                    refiner[Array.IndexOf(refiner, "centimetre")] = "";
                }
                if (refiner[i].Equals("Press") || refiner[i].Equals("press"))
                {
                    refiner[Array.IndexOf(refiner, "press")] = "breadth";
                }
                if (refiner[i].Equals("Bridge") || refiner[i].Equals("bridge"))
                {
                    refiner[Array.IndexOf(refiner, "bridge")] = "breadth";
                }
                if (refiner[i].Equals("Bread") || refiner[i].Equals("bread"))
                {
                    refiner[Array.IndexOf(refiner, "bread")] = "breadth";
                }
                if (refiner[i].Equals("Breath") || refiner[i].Equals("breath"))
                {
                    refiner[Array.IndexOf(refiner, "breath")] = "breadth";
                }
                if (refiner[i].Equals("Breast") || refiner[i].Equals("breast"))
                {
                    refiner[Array.IndexOf(refiner, "breast")] = "breadth";
                }
            }

            input = null;
            input = String.Join(" ", refiner);

            return input;
        }


        private async void StopRec()
        {
            panel1.Visible = false;
            label1.Visible = false;
            label2.Visible = true;
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;
            }
            if (waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
            }
            await Task.Delay(30);//let the wav audio file be created properly.
            int senti;
            string line = Recognition();
            // line.TrimEnd(' ');
            line = line + ".";
            senti = nlp.SentiAnalysis(line);
            Console.Text = "\n" + line;
            label2.Visible = false;
            if (senti >= 2)
            {
                SentiBox.Text = senti.ToString();
                nlp.SentenceParser(line);
                string[] depText = nlp.dependency.Split(new string[] { ")," }, StringSplitOptions.None);

                foreach (string s in depText)
                {
                    BasicDep.Text += "\r\n" + s + ")";
                }



                foreach (KeyValuePair<string, string> tt in nlp.propsUsed)
                {
                    ProcessedBox.Text += "\r\nKey is : " + tt.Key + " value is :" + tt.Value;
                }
                DrawImage(nlp.propsUsed, nlp.key);
                nlp.propsUsed.Clear();
            }
            else
            {
                ProcessedBox.Text += "\n Sentiment is negative.";
                SentiBox.Text = senti.ToString();
            }
            button1.Enabled = true;

            nlp.key = "";
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            label1.Visible = false;
            label2.Visible = true;
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;
            }
            if (waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
            }
            await Task.Delay(30);//let the wav audio file be created properly.
            int senti;
            string line = Recognition();
            // line.TrimEnd(' ');
            line = line + ".";
            senti = nlp.SentiAnalysis(line);
            Console.Text = "\n" + line;
            label2.Visible = false;
            if (senti >= 2)
            {
                SentiBox.Text =  senti.ToString();
                nlp.SentenceParser(line);
                string[] depText = nlp.dependency.Split(new string[] {"),"} , StringSplitOptions.None);

                foreach (string s in depText)
                {
                    BasicDep.Text += "\r\n" + s + ")";
                }

                
                
                foreach (KeyValuePair<string, string> tt in nlp.propsUsed)
                {
                    ProcessedBox.Text += "\r\nKey is : " + tt.Key + " value is :" + tt.Value;
                }
                DrawImage(nlp.propsUsed, nlp.key);
                nlp.propsUsed.Clear();
            }
            else
            {
                ProcessedBox.Text += "\n Sentiment is negative.";
                SentiBox.Text = senti.ToString();
            }
            button1.Enabled = true;
            
            nlp.key = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GrammarBuilder startBuilder = new GrammarBuilder();
            startBuilder.Append(new Choices("Begin")); // load verbs
            GrammarBuilder stopBuilder = new GrammarBuilder();
            stopBuilder.Append(new Choices("Stop"));

            start = new Grammar(startBuilder);
            start.Name = "start";
            stop = new Grammar(stopBuilder);
            stop.Name = "stop";

            recEngine.LoadGrammarAsync(start);
            recEngine.LoadGrammarAsync(stop);

            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;

            recEngine.Grammars[recEngine.Grammars.IndexOf(start)].Enabled = true;

            recEngine.RecognizeAsync(RecognizeMode.Multiple);

            button3.Enabled = false;
            button4.Enabled = true;
            Console.Text = "Voice command on";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
            button3.Enabled = true;
            button4.Enabled = false;
        }

        


    } // end Form1 class

    public class CoreNLP
    {
        // Path to the folder with models extracted from `stanford-corenlp-3.5.2-models.jar`
        string jarRoot = Directory.GetCurrentDirectory() + "\\stanford-corenlp-models\\";

        static StanfordCoreNLP pipeline;
        static string[] keyword = { "circle", "square", "rectangle", "triangle" };
        static bool keyFlag = false;
        public string key = null;
        Dictionary<string, string> shape;
        public Dictionary<string, string> propsUsed;
        public string dependency;
     
        public void InitializeNLP()
        {

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            Directory.SetCurrentDirectory(jarRoot);
            string curDir = Environment.CurrentDirectory;
            // Annotation pipeline configuration 
            java.util.Properties props = new java.util.Properties();

            props.setProperty("annotators", "tokenize, ssplit, parse, sentiment");
            props.setProperty("ner.useSUTime", "0");

            pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

        }

        public int SentiAnalysis(string text)
        {
            // Annotation
            var annotation = new edu.stanford.nlp.pipeline.Annotation(text);
            pipeline.annotate(annotation);

            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));

                int mainSentiment = 0;
                int longest = 0;

                String[] sentimentText = { "Very Negative", "Negative", "Neutral", "Positive", "Very Positive" };

                NumberFormat NF = new DecimalFormat("0.0000");

                var sentences = annotation.get(new CoreAnnotations.SentencesAnnotation().getClass()) as ArrayList;

                foreach (CoreMap sentence in sentences)
                {

                    Tree tree = (Tree)sentence.get(typeof(SentimentCoreAnnotations.SentimentAnnotatedTree));


                    int sentiment = edu.stanford.nlp.neural.rnn.RNNCoreAnnotations.getPredictedClass(tree);

                    String partText = sentence.ToString();

                    try
                    {

                    }
                    catch (IndexOutOfRangeException e)
                    {

                    }
                    if (partText.Length > longest)
                    {
                        mainSentiment = sentiment;
                        longest = partText.Length;
                    }

                    return sentiment;
                }

            }

            return -1;


        }

        public void SentenceParser(string sent2)
        {
            var modelsDirectory = jarRoot + @"edu\stanford\nlp\models";

            // Loading english PCFG parser from file
            var lp = LexicalizedParser.loadModel(modelsDirectory + @"\lexparser\englishPCFG.ser.gz");

            // This option shows loading and using an explicit tokenizer
            sent2.ToLower();
            var tokenizerFactory = PTBTokenizer.factory(new CoreLabelTokenFactory(), "");
            var sent2Reader = new java.io.StringReader(sent2);
            var rawWords2 = tokenizerFactory.getTokenizer(sent2Reader).tokenize();
            sent2Reader.close();
            var tree2 = lp.apply(rawWords2);

            // Extract dependencies from lexical tree

            var tlp = new PennTreebankLanguagePack();
            var gsf = tlp.grammaticalStructureFactory();
            var gs = gsf.newGrammaticalStructure(tree2);
            var tdl = gs.typedDependenciesCCprocessed();
            //Console.WriteLine("\n{0}\n", tdl);


            // Extract collapsed dependencies from parsed tree

            var tp = new TreePrint("penn,typedDependenciesCollapsed");
            tp.printTree(tree2);
           


            ArrayList dep = gs.typedDependenciesCollapsed() as ArrayList;

            foreach (TypedDependency td in dep)
            {
                for (int i = 0; i < keyword.Length; i++)
                {

                    if (td.dep().originalText().Equals(keyword[i]))
                    {

                        keyFlag = true;
                        key = keyword[i];
                        break;
                    }
                }
                if (keyFlag)
                {
                    break;
                }
            }

            keyFlag = false;


            switch (key)
            {
                case "circle":

                    Circle circle = new Circle();
                    shape = circle.GetProps();
                    propsUsed = Associator(shape, dep);

                    break;

                case "rectangle":

                    Rectangle rect = new Rectangle();
                    shape = rect.GetProps();
                    propsUsed = Associator(shape, dep);

                    break;

                case "triangle":

                    Triangle tri = new Triangle();
                    shape = tri.GetProps();
                    propsUsed = Associator(shape, dep);

                    break;

                case "square":

                    Square square = new Square();
                    shape = square.GetProps();
                    propsUsed = Associator(shape, dep);

                    break;

                default:

                    break;
            } //End of Switch

            dependency = tdl.ToString();

        } //End of SentenceParser

        public Dictionary<string, string> Associator(Dictionary<string, string> shape, ArrayList dep)
        {
            Dictionary<string, string> assoProps = new Dictionary<string, string>();
            assoProps.Clear();
            foreach (TypedDependency property in dep)
            {

                if (shape.ContainsKey(property.gov().originalText()))
                {


                    if (IsNumeric(property.dep().originalText()) && property.dep().tag() != "used")
                    {

                        shape[property.gov().originalText()] = property.dep().originalText();

                        property.dep().setTag("used");
                        assoProps.Add(property.gov().originalText(), property.dep().originalText());
                        //Edit
                        property.gov().setTag("annoted");
                    }

                    if (IsUnit(property.dep().originalText()))
                    {
                        foreach (TypedDependency unit in dep)
                        {

                            if (IsUnit(unit.gov().originalText()) && IsNumeric(unit.dep().originalText()) && unit.dep().tag() != "used")
                            {
                                shape[property.gov().originalText()] = unit.dep().originalText();

                                unit.dep().setTag("used");
                                assoProps.Add(property.gov().originalText(), unit.dep().originalText());
                                //Edit
                                property.gov().setTag("annoted");

                                break;
                            }
                        }

                    }

                }
                else if (shape.ContainsKey(property.dep().originalText()))
                {


                    if (IsNumeric(property.gov().originalText()) && property.gov().tag() != "used")
                    {

                        shape[property.dep().originalText()] = property.gov().originalText();

                        property.gov().setTag("used");
                        assoProps.Add(property.dep().originalText(), property.gov().originalText());
                        //Edit
                        property.dep().setTag("annoted");
                    }

                    if (IsUnit(property.gov().originalText()))
                    {
                        foreach (TypedDependency unit in dep)
                        {

                            if (IsUnit(unit.gov().originalText()) && IsNumeric(unit.dep().originalText()) && unit.dep().tag() != "used")
                            {

                                shape[property.dep().originalText()] = unit.dep().originalText();

                                unit.dep().setTag("used");
                                assoProps.Add(property.dep().originalText(), unit.dep().originalText());
                                //Edit
                                property.dep().setTag("annoted");
                                break;
                            }
                        }
                    }

                }


            }

            foreach (TypedDependency property in dep)
            {
                if (shape.ContainsKey(property.dep().originalText()) && property.dep().tag() != "annoted")
                {
                    shape[property.dep().originalText()] = "Find";
                    assoProps.Add(property.dep().originalText(), "Find");
                }
                
            }

            return assoProps;

        } //End Associator

        public bool IsUnit(string unit)
        {
            if (unit == "cm" || unit == "centimeter" || unit == "centimetre" || unit == "centimetres" ||
                unit == "degree" || unit == "degrees" ||
                unit == "m" || unit == "mm" || unit == "meter" || unit == "metre" ||
                unit == "sqcm" || unit == "sqm" || unit == "squarecentimeter" || unit == "squaremeter" || unit == "squarecentimetre" || unit == "squaremetre" ||
                unit == "yards")
            {
                return true;
            }

            return false;
        }

        public bool IsNumeric(string num)
        {
            double n;
            bool isNumeric = double.TryParse(num, out n);

            return isNumeric;
        }

    }



    public class Circle
    {
        public Dictionary<string, string> props = new Dictionary<string, string>();

        public Circle()
        {
            props.Add("radius", "0");
            props.Add("diameter", "0");
            props.Add("circumference", "0");
            props.Add("area", "0");
            props.Add("central", "0");
            props.Add("angle", "0");
            props.Add("arc", "0");
            props.Add("chord", "0");
            props.Add("distance", "0");

        }

        public Dictionary<string, string> GetProps()
        {

            return props;
        }

    }

    public class Square
    {
        public Dictionary<string, string> props = new Dictionary<string, string>();

        public Square()
        {
            props.Add("side", "0");
            props.Add("diagonal", "0");
            props.Add("angle", "90");
            props.Add("area", "0");
            props.Add("perimeter", "0");
            props.Add("sides", "0");
        }
        public Dictionary<string, string> GetProps()
        {

            return props;
        }


    }

    public class Rectangle
    {
        public Dictionary<string, string> props = new Dictionary<string, string>();
        public Rectangle()
        {
            props.Add("length", "0");
            props.Add("diagonal", "0");
            props.Add("angle", "90");
            props.Add("area", "0");
            props.Add("perimeter", "0");
            props.Add("breadth", "0");
            
        }
        public Dictionary<string, string> GetProps()
        {

            return props;
        }

    }


    public class Triangle
    {

        public Dictionary<string, string> props = new Dictionary<string, string>();

        public Triangle()
        {
            props.Add("sides", "0");
            props.Add("height", "0");
            props.Add("base", "0");
            props.Add("angle", "0");
            props.Add("angles", "0");
            props.Add("side", "0");
            props.Add("hypotenuse", "0");
            props.Add("area", "0");
            props.Add("perimeter", "0");
        }
        public Dictionary<string, string> GetProps()
        {

            return props;
        }


    }


}






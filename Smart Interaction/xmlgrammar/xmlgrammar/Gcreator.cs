
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Console = System.Console;

namespace xmlgrammar
{
    public class InputText
    {
        string dummyText = "Find the area of a circle whose radius is 5 cm";
        public void takeInput(string input)
        {
            dummyText = input;
        }
    }

    public class CoreNLP
    {
        // Path to the folder with models extracted from `stanford-corenlp-3.5.2-models.jar`
        string jarRoot = @"stanford-corenlp-models\";
        static StanfordCoreNLP pipeline;
        public void Init()
        {

            // Annotation pipeline configuration
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, parse, sentiment");
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);
        }

        public void POSTagger( string untagged )
        {

            string word = "";
            string key = "";
            // Annotation
            var annotation = new edu.stanford.nlp.pipeline.Annotation(untagged);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());


                var tagger = new MaxentTagger(jarRoot + @"\edu\stanford\nlp\models\pos-tagger\english-left3words\english-left3words-distsim.tagger");

                var words = MaxentTagger.tokenizeText(new java.io.StringReader(untagged)).toArray();
                foreach (ArrayList sentence in words)
                {
                    var taggedSentence = tagger.tagSentence(sentence);
                    Console.WriteLine(Sentence.listToString(taggedSentence, false));
                    string[] taggedWords = taggedSentence.ToString().Split(' ');
                    string[] slashSplit;
                    foreach (string swords in taggedWords)
                    {
                        Console.WriteLine(swords);
                        //slashSplit = swords.Split('/');
                        slashSplit = Regex.Split(swords,"/");
                        int counter = 0;
                        foreach (string stripped in slashSplit)
                        {
                            string refined = Regex.Replace(stripped, @"[^0-9a-zA-Z]+", "");
                            Console.WriteLine("Refined words: " + refined);
                            if (counter == 0)
                                word = refined;
                            else
                                key = refined;
                            counter++;
                        }
                        counter = 0;
                        AddToXML(word, key);

                    }


                }

                stream.close();
            }

        }

        public void AddToXML(string word, string key){
            switch (key)
            {
                case "CC" : //Coordinating conjunction

                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;
                case "CD" ://Cardinal number
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "DT": //Determiner
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "EX" : //Existential there
                    Console.WriteLine("Case EX Word: " + word + " " + "Key: " + key);
                    break;

                case "FW": //Foreign word
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "IN": //Preposition or subordinating conjunction
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "JJ": //Adjective
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "JJR": //Adjective, comparative
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "JJS": //Adjective, superlative
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "LS": //List item marker
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "MD": //Modal
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "NN": //Noun, singular or mass
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "NNS": //Noun, plural
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "NNP": //Proper noun, singular
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "NNPS": //Proper noun, plural
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "PDT": //Predeterminer
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "POS": //Possessive ending
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "PRP": //Personal pronoun
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "PRP$": //Possessive pronoun
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "RB": //Adverb
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "RBR": //Adverb, comparative
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "RBS": //Adverb, superlative
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "RP": //Particle
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "SYM": //Symbol
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "TO": //to
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "UH": //Interjection
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "VB": //Verb, base form
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "VBD": //Verb, past tense
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "VBG": //Verb, gerund or present participle
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "VBN": //Verb, past participle
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "VBP": //Verb, non­3rd person singular present
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "VBZ": //Verb, 3rd person singular present
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "WDT": //Wh­determiner
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "WP": //Wh­pronoun
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "WP$": //Possessive wh­pronoun
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                case "WRB": //Wh­adverb
                    Console.WriteLine("Case: " + key + " " + "Word: " + word + " " + "Key: " + key);
                    break;

                default:
                    Console.WriteLine("Could not recognize anything" + word + " " + key);
                    break;

            }
                
            
        }
    }
}

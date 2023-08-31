using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
/*using System.Collections.Generic;
using PdfSharp.Pdf;
using PdfSharp.Drawing;*/

namespace QRCode
{
    public partial class Form1 : Form
    {
        private readonly Stopwatch sw = new Stopwatch();
        private int totalCount = 0;
        private CancellationTokenSource cts;
        private readonly string carpeta;
        private readonly ConcurrentDictionary<string, byte[]> qrs = new ConcurrentDictionary<string, byte[]>();
        private string fileType;
        private readonly string keyCarpeta = "carpeta";
        private readonly string keyUltimo = "ultimo";

        public Form1()
        {
            InitializeComponent();

            // To report progress from the background worker we need to set this property
            backgroundWorker1.WorkerReportsProgress = true;
            // This event will be raised on the worker thread when the worker starts
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            // This event will be raised when we call ReportProgress
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

            // get carpeta appSetting
            carpeta = Utils.GetSetting(keyCarpeta) ?? "";
            // if there is value, adds \
            if (carpeta != "" && !carpeta.EndsWith("\\"))
            {
                carpeta += "\\";
            }
        }

        #region form events
        private void buttonGenerar_Click(object sender, EventArgs e)
        {
            // Do form validation
            if (CheckErrors())
            {
                return;
            }

            // Check codes quantity 
            if (numericUpDownCantidad.Value == 0) return;

            // Try to create folder
            try
            {
                if (!string.IsNullOrEmpty(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Initializa count and dictionary
            totalCount = 0;
            qrs.Clear();
            var fecha = dateTimePicker1.Value;
            for (int i = 0; i < numericUpDownCantidad.Value; i++)
            {
                qrs.TryAdd($"{fecha:yyyyMMddHHmmss}", null);
                fecha = fecha.AddSeconds(1);
            }

            // Start the background worker
            if (backgroundWorker1.IsBusy != true)
            {
                fileType = comboBox1.Text.ToLower();
                setControlsEnabled(false);
                // Init progress bar
                progressBar1.Value = 0;
                progressBar1.Maximum = (int)numericUpDownCantidad.Value;

                sw.Start();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                cts?.Cancel();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = 1;
            setControlsEnabled(true);
            string ultimo = Utils.GetSetting(keyUltimo);
            if (ultimo != null)
            {
                SetUltimo(ultimo);
            }
        }
        #endregion

        #region form functions
        /// <summary>
        /// Set enabled/disabled controls and WaitCursor
        /// </summary>
        /// <param name="enabled">enabled value</param>
        private void setControlsEnabled(bool enabled)
        {
            SuspendLayout();
            Application.UseWaitCursor = !enabled;
            buttonGenerar.Enabled = enabled;
            buttonCancel.Enabled = !enabled;
            buttonCancel.Cursor = Cursors.AppStarting;
            progressBar1.Enabled = !enabled;
            textBoxHTTP.Enabled = enabled;
            textBoxPrefijo.Enabled = enabled;
            numericUpDownCantidad.Enabled = enabled;
            dateTimePicker1.Enabled = enabled;
            ResumeLayout();
            Application.DoEvents();
        }

        /// <summary>
        /// Do check validation
        /// </summary>
        /// <returns>true if there are errors. false is no errors</returns>
        private bool CheckErrors()
        {
            errorProvider1.Clear(); // Clear error provider
            
            /*if (string.IsNullOrWhiteSpace(textBoxHTTP.Text)) // Check empty http text
            {
                errorProvider1.SetError(textBoxHTTP, "Valor requerido");
            } else if (!Uri.IsWellFormedUriString(textBoxHTTP.Text, UriKind.Absolute)) { // Check valid URL
                errorProvider1.SetError(textBoxHTTP, "No es una URL válida");
            }*/
            
            if (string.IsNullOrEmpty(textBoxPrefijo.Text))
            {
                errorProvider1.SetError(textBoxPrefijo, "Valor requerido");
            }

            return CountErrors(errorProvider1) > 0;
        }
        #endregion


        #region backgroundWorker events
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            cts = new CancellationTokenSource();
            ParallelOptions options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 2 * Environment.ProcessorCount,
                CancellationToken = cts.Token,
            };

            try
            {
                Parallel.ForEach(qrs, options, x =>
                {
                    byte[] qrCodeAsByteArr = Utils.GenerateQRCode($"{textBoxHTTP.Text.Trim()}{textBoxPrefijo.Text.Trim()}{x.Key}", fileType);
                    File.WriteAllBytes($"{carpeta}{x.Key}.{fileType}", qrCodeAsByteArr);
                    qrs.TryUpdate(x.Key, qrCodeAsByteArr, null);
                    backgroundWorker1.ReportProgress(Interlocked.Increment(ref totalCount));
                });

                /*if (fileType == "png")
                {
                    createPDFFile();
                }*/
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cts.Dispose();
            }
        }

        // Back on the 'UI' thread so we can update the progress bar
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            setControlsEnabled(true);
            sw.Stop();

            try
            {
                string last = qrs.Where(x => x.Value != null).Select(x => x.Key).OrderBy(x => x).ToList().Last();
                SetUltimo(last);
                Utils.SetSetting(keyUltimo, last);
#if DEBUG
                MessageBox.Show(sw.Elapsed.ToString());
#endif
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
            //label1.Text = sw.Elapsed.ToString();
        }
        #endregion

        #region private methods
        /// <summary>
        /// Set último value
        /// </summary>
        /// <param name="ultimo"></param>
        private void SetUltimo(string ultimo)
        {
            labelUltimo.Text = $"Último generado: {ultimo}";
        }

        /// <summary>
        /// Count errors in errorProvider
        /// </summary>
        /// <returns>Count of errors</returns>
        private int CountErrors(ErrorProvider errorProvider)
        {
            int errors = 0;
            foreach (Control control in Controls)
            {
                if (!string.IsNullOrWhiteSpace(errorProvider.GetError(control)))
                {
                    errors++;
                }
            }

            return errors;
        }

        /*private void createPDFFile()
        {
            var images = qrs.Where(w => w.Value != null).OrderBy(o => o.Key).Select(s => s.Value).ToList();

            //Start creating PDF
            PdfDocument doc = new PdfDocument();
            XGraphics xgr = XGraphics.FromPdfPage(doc.AddPage());
            int x = 0;
            int y = 0;
            int padding = 10;

            for (int i = 0; i < images.Count; i++)
            {
                //convert Image to XImage for PDF
                if (images[i] != null)
                {
                    using (MemoryStream stream = new MemoryStream(images[i]))
                    {
                        XImage xImg = XImage.FromStream(stream);
                        double width = xImg.PointWidth + 2 * padding;
                        int cantidadQR = (int)Math.Floor(xgr.PageSize.Width / width);
                        double xStart = (xgr.PageSize.Width - cantidadQR * width) / 2 + padding;

                        xgr.DrawImage(xImg, xStart + x * width, y * xImg.PointHeight);
                        if (xStart + (x + 2) * width >= xgr.PageSize.Width)
                        {
                            y++;
                            x = 0;
                            if ((y + 1) * xImg.PointHeight >= xgr.PageSize.Height)
                            {
                                if (images.Count > i + 1)
                                {
                                    xgr = XGraphics.FromPdfPage(doc.AddPage());
                                    y = 0;
                                }
                            }
                        }
                        else
                        {
                            x++;
                        }
                    }
                    backgroundWorker1.ReportProgress(Interlocked.Increment(ref totalCount));
                }
            }

            xgr.Dispose();

            doc.Save($"{carpeta}qrcodes.pdf");
            doc.Close();
        }*/
        #endregion
    }
}

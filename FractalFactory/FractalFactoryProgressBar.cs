using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalFactory
{
    // Fractal project management functionality.
    public partial class FractalFactory : Form
    {
        /// <summary>A collection of task-related information.</summary>
		internal class ProgressBarTask
        {
            /// <summary>Create an object for use by TaskProgressShow().</summary>
            /// <param name="text">The label text to appear next to the progress bar.</param>
            /// <param name="time">The estimated time (seconds) required by the task.</param>
            /// <remarks>You must directly initialize its "task" member.</remarks>
			public ProgressBarTask(string text, float time)
            {
                labelText = text;
                estimatedProcessingTime = time;
            }

            /// <summary>Gets/Sets the task used by TaskProgressShow(). </summary>
			public Task task { get; set; } = null!;

            /// <summary>Gets the label text to appear next to the progress bar.</summary>
			public string labelText { get; private set; } = string.Empty;

            /// <summary>Gets the estimated time (seconds) required by the task.</summary>
			public float estimatedProcessingTime { get; private set; } = 0;
        }

        internal class ProgressBarParams
        {
            public ProgressBarParams() { }
            public int steps { get; set; } = 0;
            public int sleep { get; set; } = 0;
        }

        /// <summary>Presents a progress bar simulating the progress of the associated task.</summary>
        /// <param name="pbTask">The information required to execute a task.</param>
        /// <remarks>
        /// Clearly, the task is run on a separate thread. The task typically involves
        /// an SQLite database operation having no UI feedback. So, the best we can do
        /// is present a progress bar that provides the appearance of actual progress.
        /// </remarks>
        private async void TaskProgressShow(ProgressBarTask pbTask)
        {
            ProgressBarParams pb = ProgressBarStart(pbTask.labelText, pbTask.estimatedProcessingTime);

            Debug.WriteLine($"progess bar estimated time: {pbTask.estimatedProcessingTime}");

            DateTime start = DateTime.Now;

            for (int i = 0; i < pb.steps; ++i)
            {
                if (pbTask.task.IsCompleted)
                    break;  // The task completed before exhausting the estimated time.

                ProgressBarUpdate();
                Thread.Sleep(pb.sleep);
            }

            Debug.WriteLine($"progess bar: {DateTime.Now - start}");

            // NOTE: Attempts to run tasks asynchronously can cause the application to
            // throw hard-to-understand exceptions (via FractalFactory_FormClosing())
            // when shutting it down. So, force synchronous behavior.
#if false
            // Keeping TaskHelper in the project in the event the current solution fails.
            TaskHelper.RunTaskSynchronously(pbTask.task);
#else
            pbTask.task.Wait();
#endif

            Debug.WriteLine($"await: {DateTime.Now - start}");

            ProgressBarStop();
        }

        /// <summary>Used by TaskProgressShow().</summary>
        private ProgressBarParams ProgressBarStart(string labelText, float estimatedTimeAsSeconds)
        {
            ProgressBarParams pbParams = new ProgressBarParams();

            if (estimatedTimeAsSeconds < MINIMUM_TIME)
                estimatedTimeAsSeconds = MINIMUM_TIME;

            int steps = (int)estimatedTimeAsSeconds;
            progressBar.Maximum = steps;
            progressBar.Step = 1;
            progressBar.Value = 0;

            pbParams.steps = steps;
            pbParams.sleep = (int)(estimatedTimeAsSeconds / (float)steps) * 1000;

            progressLabel.Text = labelText;
            progressPanel.Visible = true;
            progressPanel.Update();

            return pbParams;
        }

        /// <summary>Used by TaskProgressShow().</summary>
        private void ProgressBarUpdate()
        {
            progressBar.Value++;
        }

        /// <summary>Used by TaskProgressShow().</summary>
        private void ProgressBarStop()
        {
            progressPanel.Visible = false;
            progressPanel.Refresh();
        }
    }

    // Alas, my ignorance can be painful. I could not discover this solution myself.
    //    https://stackoverflow.com/questions/5095183/how-would-i-run-an-async-taskt-method-synchronously
    //
    // Why is it necessary? The UI thread must be able to update the progress
    // bar while some task (on a different thread) is being executed.
    internal static class TaskHelper
    {
        public static void RunTaskSynchronously(this Task t)
        {
            var task = Task.Run(async () => await t);
            task.Wait();
        }

        public static T RunTaskSynchronously<T>(this Task<T> t)
        {
            T res = default(T);
            var task = Task.Run(async () => res = await t);
            task.Wait();
            return res!;
        }
    }
}

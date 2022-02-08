using Utubz.Graphics;
using Utubz.Async;
using Utubz.Physics;
using Utubz.Internal.Native;
using Utubz.Internal.Native.Glad;
using Utubz.Internal.Native.Glfw;

using System;
using System.Threading;
using System.Collections.Concurrent;

namespace Utubz.Internal
{
    internal class ProcessLoop : Object
    {
        internal static void Main(string[] args, bool multithreaded = false)
        {
            Application.Main = new ProcessLoop(new DefaultEntryArgs(args), multithreaded);
            Application.Main.Start();
            Application.Main.Wait();
        }

        internal static void Main(string[] args, Type type, bool multithreaded = false)
        {
            Application.Main = new ProcessLoop(new DefaultEntryArgs(args), multithreaded);
            Application.Main.initScene = type;
            Application.Main.Start();
            Application.Main.Wait();
        }

        private class ProcessLoopStopImmediateException : Exception
        {
            public ProcessLoopStopImmediateException() : base("Process was requested to stop executing immediately.")
            {

            }
        }

        private class GlfwErrorException : Exception
        {
            public GlfwErrorException(int code, string msg) : base($"{msg} ({code})")
            {

            }
        }

        internal ConcurrentBag<Window> windows;

        public IEntryArgs Args { get; }
        public Thread MainThread { get; }
        public Thread RenderThread { get; }
        public bool QueueStop { get; set; }
        public bool Multithreaded { get; }
        public ApplicationClosingHandle OnClosing { get; set; }
        public ApplicationClosedHandle OnClosed { get; set; }
        private Type initScene;

        private bool pollingEvents;
        private bool needsRefresh;

        private void Init()
        {
            NativeUtil.InitNativeLibraries();
            Garbage.Init();
            Phy2D.Init();

            Window.Create("cool", 0, 0, 1280, 720, false, initScene);

            if (Multithreaded)
                RenderThread.Start();
        }

        private void Quit()
        {
            Utubz.Discord.Status.Quit();
            Phy2D.Quit();
            Garbage.Quit();
            NativeUtil.QuitNativeLibraries();

            Debug.Save();
        }

        private void Poll()
        {
            while (!pollingEvents) ;
            pollingEvents = true;
            glfw3.GlfwPollEvents();
            pollingEvents = false;
            while (needsRefresh) ;
        }

        /// <summary>
        /// Process Main
        /// </summary>
        private void ProcMain()
        {
            try
            {
                Init();
            } catch (Shader.ShaderCompilationException ex)
            {
                Debug.Log(ex);
                Stop();
            } catch (Exception ex)
            {
                Debug.Log("Other exception occurred");
                Debug.Log(ex);
                Stop();
            }

            while (!QueueStop)
            {
                Poll();

                foreach (Window win in windows)
                {
                    try
                    {
                        win.Update();
                    } catch (ProcessLoopStopImmediateException)
                    {
                    } catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }

            if (RenderThread.IsAlive)
                RenderThread.Join();

            Quit();
        }

        /// <summary>
        /// Render Main
        /// </summary>
        private void RenMain()
        {
            while (!QueueStop)
            {
                pollingEvents = true;
                while (pollingEvents) ;

                foreach (Window win in windows)
                {
                    try
                    {
                        win.Render();
                    } catch (ProcessLoopStopImmediateException)
                    {
                    } catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
        }

        /// <summary>
        /// Single Threaded ProcMain. Ignore for now.
        /// </summary>
        private void StProcMain()
        {
            try
            {
                Init();
            } catch (Shader.ShaderCompilationException ex)
            {
                Debug.Log(ex);
                Stop();
            } catch (Exception ex)
            {
                Debug.Log("Other exception occurred");
                Debug.Log(ex);
                Stop();
            }

            while (!QueueStop)
            {
                glfw3.GlfwPollEvents();
                UpdateAndRenderAll();
            }

            Quit();
        }

        internal void RequestRefresh()
        {
            needsRefresh = true;

            UpdateAndRenderAll();
            
            needsRefresh = false;
        }

        private void UpdateAndRenderAll()
        {
            Utubz.Discord.Status.Run();

            try
            {
                foreach (Window win in windows)
                {
                    win.Update();
                    win.Render();
                }
            } catch (ProcessLoopStopImmediateException)
            {
            } catch (Exception e)
            {
                Debug.Log(e);
            }

            Garbage.Process();
        }

        public void Wait()
        {
            while (MainThread.ThreadState != ThreadState.Stopped) Thread.Sleep(8);
        }

        public void Start()
        {
            MainThread.Start();
        }

        public void Stop()
        {
            QueueStop = true;
            OnClosing?.Invoke();
        }

        public void StopImmediate()
        {
            QueueStop = true;
            throw new ProcessLoopStopImmediateException();
        }

        private void GlfwOnError(int code, string msg)
        {
            Debug.Log(new GlfwErrorException(code, msg));
        }

        internal ProcessLoop(IEntryArgs args, bool multithreaded = false)
        {
            Args = args;
            QueueStop = false;
            windows = new ConcurrentBag<Window>();

            Multithreaded = multithreaded;

            if (multithreaded)
            {
                MainThread = new Thread(ProcMain);
                MainThread.Name = "Component Thread";
                RenderThread = new Thread(RenMain);
                RenderThread.Name = "Render Thread";
            } else
            {
                MainThread = new Thread(StProcMain);
                MainThread.Name = "Main Thread v2";
            }

            Time.asyncCtx = new TimeContext();
            Input.asyncCtx = new InputContext((int)Key.Max);

            glfw3.GlfwSetErrorCallback(GlfwOnError);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

class ThreadingStuff
{
    public static List<Thread> threads;


    public static void StartThreadingStuff()
    {
        threads = new List<Thread>();

        // Start a thread that calls a parameterized static method.
        Thread newThread = new Thread(ThreadingStuff.DoWork);
        threads.Add(newThread);
        newThread.Start(42);

        // Start a thread that calls a parameterized instance method.
        ThreadingStuff w = new ThreadingStuff();
        newThread = new Thread(w.DoMoreWork);
        threads.Add(newThread);
        newThread.Start("The answer.");
        newThread = new Thread(w.DoMoreWork);
        threads.Add(newThread);
        newThread.Start("The answer. Again");
        newThread = new Thread(w.DoMoreWork);
        threads.Add(newThread);
        newThread.Start("The answer. And Again");
    }

    public static void StopThreadingStuff()
    {
        foreach (Thread thread in threads)
        {
            thread.Abort();
        }

        Debug.Log("Threading Stuff Stopped");
    }

    public static void DoWork(object data)
    {
        Debug.Log(Environment.CurrentManagedThreadId + " Static thread procedure. Data=" + data);

        while (true)
        {
            //Uses
            Debug.Log("CPU goes BRR " + Environment.CurrentManagedThreadId);
        }
    }

    public void DoMoreWork(object data)
    {
        Debug.Log(Environment.CurrentManagedThreadId + " Instance thread procedure. Data=" + data);

        while (true)
        {
            //Uses
            Debug.Log("CPU goes BRRR " + Environment.CurrentManagedThreadId);
        }
    }

    /*// Demonstrates:
    //      ThreadLocal(T) constructor
    //      ThreadLocal(T).Value
    //      One usage of ThreadLocal(T)
    public static void StartThreadingStuff()
    {
        // Thread-Local variable that yields a name for a thread
        ThreadLocal<string> ThreadName = new ThreadLocal<string>(() =>
        {
            return "Thread" + Thread.CurrentThread.ManagedThreadId;
        });

        // Action that prints out ThreadName for the current thread
        Action action = () =>
        {
            // If ThreadName.IsValueCreated is true, it means that we are not the
            // first action to run on this thread.
            bool repeat = ThreadName.IsValueCreated;

            Debug.Log("ThreadName = " + ThreadName.Value + " " + repeat);
        };

        // Launch eight of them.  On 4 cores or less, you should see some repeat ThreadNames
        Parallel.Invoke(action, action, action, action, action, action, action, action);

        // Dispose when you are done
        ThreadName.Dispose();
    }*/

    /*[ThreadStatic]
    private static string? _requestId;

    public static void StartThreadingStuff()
    {
        Thread thread1 = new(ProcessRequest);
        Thread thread2 = new(ProcessRequest);

        thread1.Start("REQ-001");
        thread2.Start("REQ-002");

        thread1.Join();
        thread2.Join();

        Debug.Log("Main thread execution completed.");
    }

    static void ProcessRequest(object? requestId)
    {
        // Assign the request ID to the thread-static field.
        _requestId = requestId as string;

        // Simulate request processing across multiple method calls.
        PerformDatabaseOperation();
        PerformLogging();
    }

    static void PerformDatabaseOperation()
    {
        Debug.Log($"Thread {Environment.CurrentManagedThreadId}: Processing DB operation for request {_requestId}");
    }

    static void PerformLogging()
    {
        Debug.Log($"Thread {Environment.CurrentManagedThreadId}: Logging request {_requestId}");
    }*/
}
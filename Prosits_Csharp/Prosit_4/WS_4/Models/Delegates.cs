using System;

namespace WS_4.Models;

// Delegates keep the engine closed to modification while rules remain injectable.
public delegate TimeSpan ProcessingTimeDelegate(Job job);
public delegate int JobPriorityDelegate(Job job);
public delegate int JobRoutingDelegate(Job job);

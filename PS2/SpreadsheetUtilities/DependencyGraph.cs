// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 Implemented Methods. Author is Ryan Fletcher, September 2015.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// s1 depends on t1 --> t1 must be evaluated before s1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// (Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.)
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        Dictionary<String, HashSet<String>> dependees;
        Dictionary<String, HashSet<String>> dependents;
        int size;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependees = new Dictionary<string, HashSet<String>>();
            dependents = new Dictionary<string, HashSet<String>>();
            size = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (dependees.Count == 0)
                {
                    return 0;
                }
                HashSet<String> tempDependees;
                dependees.TryGetValue(s, out tempDependees);
                return tempDependees.Count;

            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if(dependents.ContainsKey(s))
            {
                HashSet<String> tempDependents;
                dependents.TryGetValue(s, out tempDependents);

                if (tempDependents.Count == 0)
                    return false;

                return true;
            }

            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependees.ContainsKey(s))
            {
                HashSet<String> tempDependees;
                dependees.TryGetValue(s, out tempDependees);

                if (tempDependees.Count == 0)
                    return false;

                return true;
            }

            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependents.ContainsKey(s))
            {
                HashSet<String> tempDependents;
                dependents.TryGetValue(s, out tempDependents);
                return tempDependents;
            }
            return null;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependents.ContainsKey(s))
            {
                HashSet<String> tempDependees;
                dependees.TryGetValue(s, out tempDependees);
                return tempDependees;
            }
            return null;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   s depends on t
        ///
        /// </summary>
        /// <param name="s"> s cannot be evaluated until t is</param>
        /// <param name="t"> t must be evaluated first.  S depends on T</param>
        public void AddDependency(string s, string t)
        {
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependents.ContainsKey(s))
            {
                HashSet<String> tempDependents;
                dependents.TryGetValue(s, out tempDependents);

                String[] dependentsToArray = tempDependents.ToArray();

                foreach (String sameDependents in dependentsToArray)
                {
                    RemoveDependency(s, sameDependents);
                }

                 foreach (String newPair in newDependents)
                {
                    AddDependency(newPair, s);
                }   
            }
            else
            {
                foreach (String newPair in newDependents)
                {
                    AddDependency(s, newPair);
                }   
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependees.ContainsKey(s))
            {
                HashSet<String> tempDependees;
                dependees.TryGetValue(s, out tempDependees);

                String[] dependeesToArray = tempDependees.ToArray();

                foreach (String sameDependees in dependeesToArray)
                {
                    RemoveDependency(sameDependees, s);
                }

                foreach (String newPair in newDependees)
                {
                    AddDependency(newPair, s);
                }
            }
            else
            {
                foreach (String newPair in newDependees)
                {
                    AddDependency(s, newPair);
                }
            }
        }

    }




}



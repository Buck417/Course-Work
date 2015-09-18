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
       private Dictionary<String, HashSet<String>> dependees;               //A dictionary that has the dependee as the string, and the dependents as a list
       private Dictionary<String, HashSet<String>> dependents;              //A dictionary that has the dependent as the string, and the dependees as a list
       private int size;                                                    //Tracks the size of each Dictionary

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
                if (dependees.ContainsKey(s))                           //If the dependees has S as a key, return the amount of dependees
                {
                    HashSet<String> tempDependees;
                    dependees.TryGetValue(s, out tempDependees);
                    return tempDependees.Count;
                }

                return 0;

            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if(dependents.ContainsKey(s))                               //If dependents has s as a key, return true if its dependees > 0
            {
                HashSet<String> tempDependents;
                dependents.TryGetValue(s, out tempDependents);

                if (tempDependents.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependees.ContainsKey(s))                               //If dependees has s as a key, return true if its dependents > 0
            {
                HashSet<String> tempDependees;
                dependees.TryGetValue(s, out tempDependees);

                if (tempDependees.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependents.ContainsKey(s))                              //If s is a key in dependents, return a Hashset of all the dependents
            {
                HashSet<String> tempDependents;
                dependents.TryGetValue(s, out tempDependents);
                foreach (String dependent in tempDependents)            //Loop through the Hashset, returning 1 dependent for each call made to the function
                {
                    if (dependent != null)
                        yield return dependent;
                }
            }
            yield break;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependees.ContainsKey(s))                               //If s is a key in dependees, return a Hashset of all the dependees
            {
                HashSet<String> tempDependees;
                dependees.TryGetValue(s, out tempDependees);
                foreach(String dependee in tempDependees){              //Loop through the Hashset, returning 1 dependee for each call made to the function
                    if(dependee != null)
                        yield return dependee;
                }
            }
            yield break;
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

            HashSet<String> tempSet;
            if (dependents.ContainsKey(s))                              //If 1 or more dependents has s as a key, create a tempSet of all the dependents with s
            {  
                dependents.TryGetValue(s, out tempSet);                 

                if (!tempSet.Contains(t))                               //If t is not a dependee, add t as a dependee to tempSet
                {
                    tempSet.Add(t);

                    if (!dependents.ContainsKey(t))                     //If t is not a key in dependents, add t to dependents with a new hashset
                    {
                        dependents.Add(t, new HashSet<string>());
                    }
                    if (dependees.ContainsKey(t))                       //If dependees has t as a key, create a tempSet of all the dependees with t, add s to the set
                    {
                        dependees.TryGetValue(t, out tempSet);
                        tempSet.Add(s);
                    }
                    else                                                //Else create a tempSet  and add s to it and add t, tempset to dependees, creating a new pair
                    {
                        tempSet = new HashSet<string>();
                        tempSet.Add(s);
                        dependees.Add(t, tempSet);
                    }
                    size++;
                }
                else
                {
                    return;
                }
            }
            else
                                                                        //If there is no s key, add s and t to dependents and dependees, effectively a new entry
            {
                tempSet = new HashSet<string>();
                tempSet.Add(t);
                dependents.Add(s, tempSet);
                dependees.Add(s, new HashSet<string>());

                if (!dependents.ContainsKey(t))
                {
                    dependents.Add(t, new HashSet<string>());
                }
                if (dependees.ContainsKey(t))
                {
                    dependees.TryGetValue(t, out tempSet);
                    tempSet.Add(s);
                }
                else
                {
                    tempSet = new HashSet<string>();
                    tempSet.Add(s);
                    dependees.Add(t, tempSet);
                }
                size++;
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            HashSet<String> tempDependents;
            HashSet<String> tempDependees;
            if (dependents.ContainsKey(s))                                      //If dependents contains a key s, create a temp with all dependents with s
            {
                dependents.TryGetValue(s, out tempDependents);

                if (tempDependents.Contains(t))                                 //If t or s is a dependee, remove it
                {
                    tempDependents.Remove(t);
                    dependees.TryGetValue(t, out tempDependees);
                    tempDependees.Remove(s);

                    if (tempDependees.Count == 0 & tempDependents.Count == 0)       //If no more dependees exist, then remove the dependent/dependee 
                    {
                        dependees.Remove(s);
                        dependents.Remove(s);
                    }

                    if (s != t)                                                     //Then remove the dependee/dependent from the opposite dictionaries
                    {
                        dependents.TryGetValue(t, out tempDependents);
                        dependees.TryGetValue(t, out tempDependees);

                        if (tempDependees.Count == 0 & tempDependents.Count == 0)
                        {
                            dependents.Remove(t);
                            dependees.Remove(t);
                        }
                    }
                    size--;

                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            HashSet<String> tempDependents;
            if (dependents.ContainsKey(s))
            {

                dependents.TryGetValue(s, out tempDependents);

                String[] dependentsToArray = tempDependents.ToArray();

                foreach (String sameDependents in dependentsToArray)                            //Remove each matching dependent from s and add the new dependent
                {
                    RemoveDependency(s, sameDependents);
                }

                 foreach (String newItem in newDependents)
                {
                    AddDependency(s, newItem);
                }   
            }
            else
            {
                foreach (String newItem in newDependents)                                       //No matching dependent exists, so add it
                {
                    AddDependency(s, newItem);
                }   
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            HashSet<String> tempDependees;
            if (dependees.ContainsKey(s))
            {
                
                dependees.TryGetValue(s, out tempDependees);

                String[] dependeesToArray = tempDependees.ToArray();

                foreach (String sameDependees in dependeesToArray)                              //Remove each matching dependee from s and add the new dependee
                {
                    RemoveDependency(sameDependees, s);
                }

                foreach (String newItem in newDependees)
                {
                    AddDependency(newItem, s);
                }
            }
            else
            {
                foreach (String newItem in newDependees)                                        //No matching dependee exists, so add it
                {
                    AddDependency(s, newItem);
                }
            }
        }

    }




}



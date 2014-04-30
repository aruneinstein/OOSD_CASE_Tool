using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Represents a State in a State Transition Diagram.
    /// </summary>
    public class State
    {
        public const string END_STATE = "End State";
        public const string START_STATE = "Start State";
        public const string STATE = "State";

        /// <summary>Name of this State.</summary>
        public string Name { get; private set; }

        /// <summary>
        /// Type of State.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>List of Transitions possible from this State.</summary>
        private List<Transition> transitions;

        /// <summary>
        /// Creates a new State.
        /// </summary>
        /// <param name="name">Name of this State.</param>
        /// <param name="type">Type of this State.</param>
        public State(string name, string type)
        {
            this.Name = name;
            this.Type = type;
            transitions = new List<Transition>();
        }

        /// <summary>
        /// Adds a new Next State to this list of Next States.
        /// </summary>
        /// <param name="data">Data associated with the Next State Transition.</param>
        /// <param name="operation">Operation that causes the State Transition.</param>
        /// <param name="nextState">Next State.</param>
        /// <returns>False if nextState already exists in this list, else adds
        /// the new nextState and returns true.</returns>
        public bool addNextState(string data, string operation, State nextState)
        {
            // if the next state name is already in this state's list of next states,
            // don't add and return false
            if (nextStateExists(nextState.Name))
            {
                return false;
            }

            Transition transition = new Transition(data, operation, nextState);
            transitions.Add(transition);

            return true;
        }

        /// <summary>
        /// Returns true if the State given is in this State's list of Next States.
        /// </summary>
        /// <param name="name">Name of State to check.</param>
        /// <returns>True if the State is in this State's list of Next States.</returns>
        public bool nextStateExists(string name)
        {
            return getNextStateNames().Contains(name);
        }

        /// <summary>
        /// Returns the list of Next States from this State.
        /// </summary>
        /// <returns>Returns the list of Next States from this State.</returns>
        public List<State> getNextStates()
        {
            List<State> nextStates = new List<State>();
            foreach (Transition t in transitions)
            {
                nextStates.Add(t.NextState);
            }

            return nextStates;
        }

        /// <summary>
        /// Returns the List of Transactions that encapsulates the Next States.
        /// </summary>
        /// <returns></returns>
        public List<Transition> getTransitions()
        {
            List<Transition> transitionsCopy = new List<Transition>();
            foreach (Transition t in transitions)
            {
                transitionsCopy.Add(t);
            }
            return transitionsCopy;
        }

        /// <summary>
        /// Returns the list of Next State Names.
        /// </summary>
        /// <returns></returns>
        public List<string> getNextStateNames()
        {
            List<string> names = new List<string>();
            foreach (Transition t in transitions)
            {
                names.Add(t.NextState.Name);
            }

            return names;
        }

        /// <summary>
        /// Returns the number of Transitions possible from this State.
        /// </summary>
        /// <returns>
        /// Returns the number of Transitions possible from this State.
        /// </returns>
        public int getNextStateCount()
        {
            return transitions.Count;
        }



        /// <summary>
        /// Represents a Transition between States.
        /// </summary>
        public class Transition
        {
            /// <summary>Event/data that is associated with a transition.</summary>
            public string Data { get; private set; }

            /// <summary>Command/operation that causes the transition.</summary>
            public string Operation { get; private set; }

            /// <summary>Next state as a result of the transition.</summary>
            public State NextState { get; private set; }

            /// <summary>
            /// Creates a Transition that represents the transition from one State
            /// to another State of a State Transition Diagram.
            /// </summary>
            /// <param name="data">Data associated with the Transition.</param>
            /// <param name="operation">The operation/command that caused the Transition.</param>
            /// <param name="nextState">The next state.</param>
            public Transition(string data, string operation, State nextState)
            {
                this.Data = data;
                this.Operation = operation;
                this.NextState = nextState;
            }
        }
        
    }
}

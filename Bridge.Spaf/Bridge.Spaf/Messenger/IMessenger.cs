using System;

namespace Bridge.Messenger
{
    public interface IMessenger
    {
        /// <summary>
        /// Send Message with args
        /// </summary>
        /// <typeparam name="TSender">TSender</typeparam>
        /// <typeparam name="TArgs">TMessageArgs</typeparam>
        /// <param name="sender">Sender</param>
        /// <param name="message">Message</param>
        /// <param name="args">Args</param>
        void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class;

        /// <summary>
        /// Send Message without args
        /// </summary>
        /// <typeparam name="TSender">TSender</typeparam>
        /// <param name="sender">Sender</param>
        /// <param name="message">Message</param>
        void Send<TSender>(TSender sender, string message) where TSender : class;

        /// <summary>
        /// Subscribe Message with args
        /// </summary>
        /// <typeparam name="TSender">TSender</typeparam>
        /// <typeparam name="TArgs">TArgs</typeparam>
        /// <param name="subscriber">Subscriber</param>
        /// <param name="message">Message</param>
        /// <param name="callback">Action</param>
        /// <param name="source">source</param>
        void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback,
            TSender source = null) where TSender : class;

        /// <summary>
        /// Subscribe Message without args
        /// </summary>
        /// <typeparam name="TSender">TSender</typeparam>
        /// <param name="subscriber">Subscriber</param>
        /// <param name="message">Message</param>
        /// <param name="callback">Action</param>
        /// <param name="source">source</param>
        void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback,
            TSender source = null) where TSender : class;

        /// <summary>
        /// Unsubscribe action with args
        /// </summary>
        /// <typeparam name="TSender">TSender</typeparam>
        /// <typeparam name="TArgs">TArgs</typeparam>
        /// <param name="subscriber">Subscriber</param>
        /// <param name="message">Message</param>
        void Unsubscribe<TSender, TArgs>(object subscriber, string message) where TSender : class;

        /// <summary>
        /// Unsubscribe action without args
        /// </summary>
        /// <typeparam name="TSender">TSender</typeparam>
        /// <param name="subscriber">Subscriber</param>
        /// <param name="message">Message</param>
        void Unsubscribe<TSender>(object subscriber, string message) where TSender : class;

        /// <summary>
        /// Remove all callbacks
        /// </summary>
        void ResetMessenger();
    }
}
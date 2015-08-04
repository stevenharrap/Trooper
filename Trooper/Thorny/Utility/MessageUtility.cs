//--------------------------------------------------------------------------------------
// <copyright file="ErrorUtility.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Thorny.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Thorny;
    using Trooper.Thorny.Business.Response;

    /// <summary>
    /// The error utility for creating and converting errors
    /// </summary>
    public class MessageUtility
    {
        private readonly MessageAlertLevel level;
        
        public static MessageUtility Errors { get; private set; }

        public static MessageUtility Warnings { get; private set; }

        public static MessageUtility Notes { get; private set; }

        public static MessageUtility Success { get; private set; }

        public static MessageUtility Messages(MessageAlertLevel level)
        {
            switch (level)
            {
                case MessageAlertLevel.Error:
                    return Errors;
                case MessageAlertLevel.Warning:
                    return Warnings;
                case MessageAlertLevel.Note:
                    return Notes;
                case MessageAlertLevel.Success:
                    return Success;
            }

            return null;
        }

        static MessageUtility()
        {
            Errors = new MessageUtility(MessageAlertLevel.Error);
            Warnings = new MessageUtility(MessageAlertLevel.Warning);
            Notes = new MessageUtility(MessageAlertLevel.Note);
            Success = new MessageUtility(MessageAlertLevel.Success);
        }

        private MessageUtility(MessageAlertLevel level)
        {
            this.level = level;
        }

        public Message Make<TEntity>(string message, string code, Expression<Func<TEntity>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            var propertyInfo = memberExpression == null ? null : memberExpression.Member;

            return new Message
            {
                Content = message,
                Code = code,
                Entity = memberExpression.Expression.GetType().FullName,
                Property = propertyInfo.Name,
                Level = this.level
            };
        }

        /// <summary>
        /// Make an error for a specific entity
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="property">
        /// The property which caused the error
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of entity
        /// </typeparam>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        public Message Make<TEntity>(string message, string code, TEntity entity, string property)
            where TEntity : class
        {
            return new Message
                       {
                           Content = message,
                           Code = code,
                           Entity = entity == null ? string.Empty : entity.GetType().FullName,
                           Property = property,
                           Level = this.level
                       };
        }

        /// <summary>
        /// Make an error
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="property">
        /// The property that caused the error.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        public Message Make(string message, string code, string property)
        {
            return new Message
            {
                Content = message,
                Code = code,
                Entity = null,
                Property = property,
                Level = this.level
            };
        }

        /// <summary>
        /// Make an error
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        public Message Make(string message, string code)
        {
            return new Message
            {
                Content = message,
                Code = code,
                Entity = null,
                Property = null,
                Level = this.level
            };
        }
        
        /// <summary>
        /// Make an error and add it to the response then set the Ok to false.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="property">
        /// The property that caused the error.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity
        /// </typeparam>
        /// <returns>
        /// Returns the operation response.
        /// </returns>
        public IResponse Add<TEntity>(string message, string code, TEntity entity, string property, IResponse response)
            where TEntity : class
        {
            var e = Make(message, code, entity, property);

            if (response.Messages == null)
            {
                response.Messages = new List<Message>();
            }

            response.Messages.Add(e);

            return response;
        }

        /// <summary>
        /// Make an error and add it to the response then set the Ok to false.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="property">
        /// The property that caused the error.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// Returns the operation response.
        /// </returns>
        public IResponse Add(string message, string code, string property, IResponse response)
        {
            var e = Make(message, code, property);

            if (response.Messages == null)
            {
                response.Messages = new List<Message>();
            }

            response.Messages.Add(e);

            return response;
        }

        /// <summary>
        /// Make an error and add it to the response then set the Ok to false.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// Returns the operation response.
        /// </returns>
        public IResponse Add(string message, string code, IResponse response)
        {
            if (response == null)
            {
                response = new Response();
            }

            var e = Make(message, code);

            if (response.Messages == null)
            {
                response.Messages = new List<Message>();
            }

            response.Messages.Add(e);

            return response;
        }

        public static MessageAlertLevel? GetWorstMessageLevel(IList<Message> messages)
        {
            if (messages == null || !messages.Any())
            {
                return null;
            }

            if (messages.Any(m => m != null && m.Level == MessageAlertLevel.Error))
            {
                return MessageAlertLevel.Error;
            }

            if (messages.Any(m => m != null && m.Level == MessageAlertLevel.Warning))
            {
                return MessageAlertLevel.Warning;
            }

            if (messages.Any(m => m != null && m.Level == MessageAlertLevel.Note))
            {
                return MessageAlertLevel.Note;
            }

            if (messages.Any(m => m != null && m.Level == MessageAlertLevel.Success))
            {
                return MessageAlertLevel.Success;
            }

            return null;
        }

        public static MessageAlertLevel? GetWorstMessageLevel(IList<MessageAlertLevel?> levels)
        {
            if (levels == null || !levels.Any())
            {
                return null;
            }

            if (levels.Any(m => m != null && m == MessageAlertLevel.Error))
            {
                return MessageAlertLevel.Error;
            }

            if (levels.Any(m => m != null && m == MessageAlertLevel.Warning))
            {
                return MessageAlertLevel.Warning;
            }

            if (levels.Any(m => m != null && m == MessageAlertLevel.Note))
            {
                return MessageAlertLevel.Note;
            }

            if (levels.Any(m => m != null && m == MessageAlertLevel.Success))
            {
                return MessageAlertLevel.Success;
            }

            return null;
        }

		/// <summary>
		///	Are all the message either sucess or notes
		/// </summary>
		/// <param name="messages"></param>
		/// <returns></returns>
	    public static bool IsOk(IList<Message> messages)
	    {
		    if (messages == null)
		    {
			    return true;
		    }

		    return messages.All(m => m.Level == MessageAlertLevel.Success || m.Level == MessageAlertLevel.Note);
	    }

		/// <summary>
		/// Is at least one of the messages are warning but none error
		/// </summary>
		/// <param name="messages"></param>
		/// <returns></returns>
		public static bool IsWarning(IList<Message> messages)
		{
			if (messages == null)
			{
				return false;
			}

			return messages.Any(m => m.Level == MessageAlertLevel.Warning) && messages.All(m => m.Level != MessageAlertLevel.Error);
		}

		/// <summary>
		/// Is there at least one error
		/// </summary>
		/// <param name="messages"></param>
		/// <returns></returns>
		public static bool IsError(IList<Message> messages)
		{
			if (messages == null)
			{
				return false;
			}

			return messages.Any(m => m.Level == MessageAlertLevel.Error);
		}

        /// <summary>
        /// The add the messages to the response and set Ok to false if there where actually
        /// anything in the errors parameter. Then returns response.
        /// </summary>
        /// <param name="messages">
        /// The messages.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// Returns the operation response.
        /// </returns>
        public static IResponse Add(IList<Message> messages, IResponse response)
        {
            if (response == null || messages == null || !messages.Any())
            {
                return response;
            }

            if (response.Messages == null)
            {
                response.Messages = new List<Message>();
            }

            response.Messages.AddRange(messages);

            return response;
        }

        /// <summary>
        /// Copy the messages from response1 into response2 and returns response2
        /// </summary>
        /// <param name="response1">
        /// The response 1.
        /// </param>
        /// <param name="response2">
        /// The response 2.
        /// </param>
        /// <returns>
        /// Returns the operation response.
        /// </returns>
        public static IResponse Add(IResponse response1, IResponse response2)
        {
            if (response1 == null || response2 == null)
            {
                return response2;
            }

            return Add(response1.Messages, response2);
        }

        public static TiResponse Add<TiResponse>(IResponse response1, TiResponse response2)
            where TiResponse : IResponse
        {
            if (response1 == null || response2 == null)
            {
                return response2;
            }

            Add(response1.Messages, response2);

            return response2;
        }
    }
}
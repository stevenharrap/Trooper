//--------------------------------------------------------------------------------------
// <copyright file="ErrorUtility.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    
    using Trooper.BusinessOperation2;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using Trooper.BusinessOperation2.OperationResponse;

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

        public IMessage Make<TEntity>(string message, Expression<Func<TEntity>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            var propertyInfo = memberExpression == null ? null : memberExpression.Member;

            return new Message
            {
                Content = message,
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
        public IMessage Make<TEntity>(string message, TEntity entity, string property)
            where TEntity : class
        {
            return new Message
                       {
                           Content = message,
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
        public IMessage Make(string message, string property)
        {
            return new Message
            {
                Content = message,
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
        public IMessage Make(string message)
        {
            return new Message
            {
                Content = message,
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
        public IResponse Add<TEntity>(string message, TEntity entity, string property, IResponse response)
            where TEntity : class
        {
            var e = Make(message, entity, property);

            if (response.Messages == null)
            {
                response.Messages = new List<IMessage>();
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
        public IResponse Add(string message, string property, IResponse response)
        {
            var e = Make(message, property);

            if (response.Messages == null)
            {
                response.Messages = new List<IMessage>();
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
        public IResponse Add(string message, IResponse response)
        {
            if (response == null)
            {
                response = new Response();
            }

            var e = Make(message);

            if (response.Messages == null)
            {
                response.Messages = new List<IMessage>();
            }

            response.Messages.Add(e);

            return response;
        }

        public static MessageAlertLevel? GetWorstMessageLevel(List<IMessage> messages)
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

        public static MessageAlertLevel? GetWorstMessageLevel(List<MessageAlertLevel?> levels)
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
        /// The add the errors to the response and set Ok to false if there where actually
        /// anything in the errors parameter.
        /// </summary>
        /// <param name="messages">
        /// The errors.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// Returns the operation response.
        /// </returns>
        public static IResponse Add(List<IMessage> messages, IResponse response)
        {
            if (response == null || messages == null || !messages.Any())
            {
                return response;
            }

            if (response.Messages == null)
            {
                response.Messages = new List<IMessage>();
            }

            response.Messages.AddRange(messages);

            return response;
        }

        /// <summary>
        /// Copy the errors from response1 into response2
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
    }
}
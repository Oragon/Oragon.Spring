#region License

/*
 * Copyright © 2002-2011 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

using Common.Logging;

using Oragon.Spring.Core;
using Oragon.Spring.Core.TypeConversion;
using Oragon.Spring.Objects.Factory.Config;
using Oragon.Spring.Util;
using Oragon.Spring.Expressions;

#endregion

namespace Oragon.Spring.Objects.Factory.Support
{
    /// <summary>
    /// Concrete implementation of the
    /// <see cref="Oragon.Spring.Objects.Factory.IListableObjectFactory"/> and
    /// <see cref="Oragon.Spring.Objects.Factory.Support.IObjectDefinitionRegistry"/>
    /// interfaces.
    /// </summary>
    /// <remarks>
    /// <p>
    /// This class is a full-fledged object factory based on object definitions
    /// that is usable straight out of the box.
    /// </p>
    /// <p>
    /// Can be used as an object factory in and of itself, or as a superclass
    /// for custom object factory implementations. Note that readers for
    /// specific object definition formats are typically implemented separately
    /// rather than as object factory subclasses.
    /// </p>
    /// <p>
    /// For an alternative implementation of the
    /// <see cref="Oragon.Spring.Objects.Factory.IListableObjectFactory"/> interface,
    /// have a look at the
    /// <see cref="Oragon.Spring.Objects.Factory.Support.StaticListableObjectFactory"/>
    /// class, which manages existing object instances rather than creating new
    /// ones based on object definitions.
    /// </p>
    /// </remarks>
    /// <author>Juergen Hoeller</author>
    /// <author>Rick Evans (.NET)</author>
    /// <seealso cref="Oragon.Spring.Objects.Factory.Xml.XmlObjectDefinitionReader"/>
    [Serializable]
    public class DefaultListableObjectFactory :
        AbstractAutowireCapableObjectFactory,
        IConfigurableListableObjectFactory,
        IObjectDefinitionRegistry
    {
        #region Constructor (s) / Destructor

        /// <summary>
        /// Creates a new instance of the
        /// <see cref="Oragon.Spring.Objects.Factory.Support.DefaultListableObjectFactory"/> class.
        /// </summary>
        public DefaultListableObjectFactory()
            : this(true, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the
        /// <see cref="Oragon.Spring.Objects.Factory.Support.DefaultListableObjectFactory"/> class.
        /// </summary>
        /// <param name="caseSensitive">Flag specifying whether to make this object factory case sensitive or not.</param>
        public DefaultListableObjectFactory(bool caseSensitive)
            : this(caseSensitive, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the
        /// <see cref="Oragon.Spring.Objects.Factory.Support.DefaultListableObjectFactory"/> class.
        /// </summary>
        /// <param name="parentFactory">The parent object factory.</param>
        public DefaultListableObjectFactory(IObjectFactory parentFactory)
            : this(true, parentFactory)
        {
        }

        /// <summary>
        /// Creates a new instance of the
        /// <see cref="Oragon.Spring.Objects.Factory.Support.DefaultListableObjectFactory"/> class.
        /// </summary>
        /// <param name="caseSensitive">Flag specifying whether to make this object factory case sensitive or not.</param>
        /// <param name="parentFactory">The parent object factory.</param>
        public DefaultListableObjectFactory(bool caseSensitive, IObjectFactory parentFactory)
            : base(caseSensitive, parentFactory)
        {
            AllowObjectDefinitionOverriding = true;
            if (caseSensitive)
            {
                objectDefinitionMap = new Dictionary<string, IObjectDefinition>();
            }
            else
            {
                objectDefinitionMap = new Dictionary<string, IObjectDefinition>(StringComparer.OrdinalIgnoreCase);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Should object definitions registered under the same name as an
        /// existing object definition be allowed?
        /// </summary>
        /// <remarks>
        /// <p>
        /// If <see langword="true"/>, then the new object definition will
        /// replace (override) the existing object definition. If
        /// <see langword="false"/>, an exception will be thrown when
        /// an attempt is made to register an object definition under the same
        /// name as an already existing object definition.
        /// </p>
        /// <p>
        /// The default is <see langword="true"/>.
        /// </p>
        /// </remarks>
        /// <value>
        /// <see langword="true"/> is the registration of an object definition
        /// under the same name as an existing object definition is allowed.
        /// </value>
        public bool AllowObjectDefinitionOverriding { get; set; }


        /// <summary>
        /// Get or set custom autowire candidate resolver for this IObjectFactory to use
        /// when deciding whether a bean definition should be considered as a
        /// candidate for autowiring.  Never <code>null</code>
        /// </summary>
        public IAutowireCandidateResolver AutowireCandidateResolver
        {
            get
            {
                return autowireCandidateResolver;
            }
            set
            {
                AssertUtils.ArgumentNotNull(value, "AutowireCandidateResolver");
                autowireCandidateResolver = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find object instances that match the <paramref name="requiredType"/>.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Called by autowiring. If a subclass cannot obtain information about object
        /// names by <see cref="System.Type"/>, a corresponding exception should be thrown.
        /// </p>
        /// </remarks>
        /// <param name="requiredType">
        ///   The type of the objects to look up.
        /// </param>
        /// <returns>
        /// An <see cref="System.Collections.IDictionary"/> of object names and object
        /// instances that match the <paramref name="requiredType"/>, or
        /// <see langword="null"/> if none is found.
        /// </returns>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// In case of errors.
        /// </exception>
        protected override IDictionary<string, object> FindMatchingObjects(Type requiredType)
        {
            return ObjectFactoryUtils.ObjectsOfTypeIncludingAncestors(
                this, requiredType, true, true);
        }

        /// <summary>
        /// Return the names of the objects that depend on the given object.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Called by the
        /// <see cref="Oragon.Spring.Objects.Factory.Support.AbstractObjectFactory.DestroyObject"/>
        /// so that dependant objects are able to be disposed of first.
        /// </p>
        /// </remarks>
        /// <param name="objectName">
        /// The name of the object to find depending objects for.
        /// </param>
        /// <returns>
        /// The array of names of depending objects, or the empty string array if none.
        /// </returns>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// In case of errors.
        /// </exception>
        protected override IList<string> GetDependingObjectNames(string objectName)
        {
            List<string> dependingObjectNames = new List<string>();
			if (dependingObjectNamesCache == null)
			{
				dependingObjectNamesCache = new Dictionary<string, List<string>>();
				IList<string> allObjectDefinitionNames = GetObjectDefinitionNames();
				foreach (string name in allObjectDefinitionNames)
				{
					if (ContainsObjectDefinition(name))
					{
						RootObjectDefinition rod = GetMergedObjectDefinition(name, false);
						if (rod.DependsOn != null)
						{
							foreach (var dependsOnName in rod.DependsOn)
							{
								if (!dependingObjectNamesCache.TryGetValue(dependsOnName, out dependingObjectNames))
								{
									dependingObjectNames = new List<string>();
									dependingObjectNamesCache.Add(dependsOnName, dependingObjectNames);
								}
								dependingObjectNames.Add(name);
							}
						}
					}
				}
			}
			if (dependingObjectNamesCache.TryGetValue(objectName, out dependingObjectNames))
				return dependingObjectNames;
			return new string[]{};
		}

        /// <summary>
        /// Check whether the specified object matches the supplied <paramref name="type"/>.
        /// </summary>
        /// <param name="objectName">The name of the object to check.</param>
        /// <param name="type">
        /// The <see cref="System.Type"/> to check for.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the object matches the supplied <paramref name="type"/>,
        /// or if the supplied <paramref name="type"/> is <see langword="null"/>.
        /// </returns>
        private bool IsObjectTypeMatch(string objectName, Type type)
        {
            if (type == null)
            {
                return true;
            }
            Type objectType = GetType(objectName);
            return (objectType != null && type.IsAssignableFrom(objectType));
        }

        private bool IsObjectDefinitionTypeMatch(string name, Type checkedType)
        {
            return IsObjectDefinitionTypeMatch(name, checkedType, false);
        }

        private bool IsObjectDefinitionTypeMatch(string name, Type checkedType, bool includeAncestor)
        {
            if (checkedType == null)
            {
                return true;
            }
            RootObjectDefinition rod = GetMergedObjectDefinition(name, includeAncestor);
            return (rod.HasObjectType && checkedType.IsAssignableFrom(rod.ObjectType));
        }

        #endregion

        #region Fields

        /// <summary>
        /// The mapping of object depending object names, keyed by object name.
        /// </summary>
		private IDictionary<string, List<string>> dependingObjectNamesCache;

        /// <summary>
        /// The <see cref="Common.Logging.ILog"/> instance for this class.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(DefaultListableObjectFactory));

        /// <summary>
        /// The mapping of object definition objects, keyed by object name.
        /// </summary>
        private readonly IDictionary<string, IObjectDefinition> objectDefinitionMap;

        /// <summary>
        /// List of object definition names, in registration order.
        /// </summary>
        private readonly List<string> objectDefinitionNames = new List<string>();

        /// <summary>
        /// Resolver to use for checking if an object definition is an autowire candidate
        /// </summary>
        private IAutowireCandidateResolver autowireCandidateResolver = AutowireUtils.CreateAutowireCandidateResolver();

        /// <summary>
        /// IDictionary from dependency type to corresponding autowired value 
        /// </summary>
        private readonly IDictionary<Type, object> resolvableDependencies = new Dictionary<Type, object>();

        #endregion

        #region IObjectDefinitionRegistry Members

        /// <summary>
        /// Return the number of objects defined in this registry.
        /// </summary>
        /// <value>
        /// The number of objects defined in this registry.
        /// </value>
        /// <seealso cref="Oragon.Spring.Objects.Factory.Support.IObjectDefinitionRegistry.ObjectDefinitionCount"/>
        public int ObjectDefinitionCount
        {
            get { return objectDefinitionMap.Count; }
        }

        /// <summary>
        /// Check if this registry contains a object definition with the given
        /// name.
        /// </summary>
        /// <param name="name">
        /// The name of the object to look for.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this object factory contains an object
        /// definition with the given name.
        /// </returns>
        /// <seealso cref="Oragon.Spring.Objects.Factory.Support.IObjectDefinitionRegistry.ContainsObjectDefinition(string)"/>
        public override bool ContainsObjectDefinition(string name)
        {
            return objectDefinitionMap.ContainsKey(name);
        }

        /// <summary>
        /// Register a new object definition with this registry.
        /// </summary>
        /// <param name="name">
        /// The name of the object instance to register.
        /// </param>
        /// <param name="objectDefinition">
        /// The definition of the object instance to register.
        /// </param>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// If the object definition is invalid.
        /// </exception>
        /// <seealso cref="Oragon.Spring.Objects.Factory.Support.IObjectDefinitionRegistry.RegisterObjectDefinition(string, IObjectDefinition)"/>
        public override void RegisterObjectDefinition(
            string name, IObjectDefinition objectDefinition)
        {
            if (objectDefinition is AbstractObjectDefinition)
            {
                try
                {
                    ((AbstractObjectDefinition)objectDefinition).Validate();
                }
                catch (ObjectDefinitionValidationException ex)
                {
                    throw new ObjectDefinitionStoreException(
                        objectDefinition.ResourceDescription,
                        name,
                        "Validation of object definition failed.",
                        ex);
                }
            }
            IObjectDefinition oldObjectDefinition;
            if (objectDefinitionMap.TryGetValue(name, out oldObjectDefinition))
            {
                if (!AllowObjectDefinitionOverriding)
                {
                    throw new ObjectDefinitionStoreException(
                        string.Format(
                            "Cannot register object definition [{0}] for object '{1}': there's already [{2}] bound.",
                            objectDefinition, name, oldObjectDefinition));
                }
                else
                {
                    #region Instrumentation

                    if (log.IsDebugEnabled)
                    {
                        log.Debug(
                            string.Format(
                                "Overriding object definition for object '{0}': replacing [{1}] with [{2}].",
                                name, oldObjectDefinition, objectDefinition));
                    }

                    #endregion
                }
            }
            else
            {
                objectDefinitionNames.Add(name);
            }
            objectDefinitionMap[name] = objectDefinition;
        }

        #endregion

        #region IConfigurableListableObjectFactory Members

        /// <summary>
        /// Ensure that all non-lazy-init singletons are instantiated, also
        /// considering <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s.
        /// </summary>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// If one of the singleton objects could not be created.
        /// </exception>
        /// <seealso cref="Oragon.Spring.Objects.Factory.Config.IConfigurableListableObjectFactory.PreInstantiateSingletons"/>
        public void PreInstantiateSingletons()
        {
            #region Instrumentation

            if (log.IsDebugEnabled)
            {
                log.Debug("Pre-instantiating singletons in factory [" + this + "]");
            }

            #endregion

            try
            {
                int definitionCount = objectDefinitionNames.Count;
                for (int i = 0; i < definitionCount; i++)
                {
                    string name = objectDefinitionNames[i];
                    if (!ContainsSingleton(name) && ContainsObjectDefinition(name))
                    {
                        RootObjectDefinition definition
                            = GetMergedObjectDefinition(name, false);
                        if (!definition.IsAbstract
                            && definition.IsSingleton
                            && !definition.IsLazyInit)
                        {
                            Type objectType = ResolveObjectType(definition, name);
                            if (objectType != null
                                && typeof(IFactoryObject).IsAssignableFrom(definition.ObjectType))
                            {
                                IFactoryObject factoryObject = (IFactoryObject)GetObject(
                                                                                    ObjectFactoryUtils.
                                                                                        BuildFactoryObjectName(name));
                                if (factoryObject.IsSingleton)
                                {
                                    GetObject(name);
                                }
                            }
                            else
                            {
                                GetObject(name);
                            }
                        }
                    }
                }
            }
            catch (ObjectsException)
            {
                // destroy already created singletons to avoid dangling resources...
                try
                {
                    Dispose();
                }
                catch (Exception ex)
                {
                    log.Error(
                        "PreInstantiateSingletons failed but couldn't destroy any already-created singletons.",
                        ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Register a special dependency type with corresponding autowired value.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency to register.
        /// This will typically be a base interface such as IObjectFactory, with extensions of it resolved
        /// as well if declared as an autowiring dependency (e.g. IListableBeanFactory),
        /// as long as the given value actually implements the extended interface.</param>
        /// <param name="autowiredValue">The autowired value.  This may also be an
        /// implementation o the <see cref="IObjectFactory"/> interface,
        /// which allows for lazy resolution of the actual target value.</param>
        /// <remarks>
        /// This is intended for factory/context references that are supposed
        /// to be autowirable but are not defined as objects in the factory:
        /// e.g. a dependency of type ApplicationContext resolved to the
        /// ApplicationContext instance that the object is living in.
        /// <para>
        /// Note there are no such default types registered in a plain IObjectFactory,
        /// not even for the IObjectFactory interface itself.
        /// </para>
        /// </remarks>
        public void RegisterResolvableDependency(Type dependencyType, object autowiredValue)
        {
            AssertUtils.ArgumentNotNull(dependencyType, "dependencyType");
            if (autowiredValue != null)
            {
                AssertUtils.IsTrue((autowiredValue is IObjectFactory) || dependencyType.IsInstanceOfType(autowiredValue),
                    "Value [" + autowiredValue + "] does not implement specified type [" + dependencyType.Name + "]");
                if (!resolvableDependencies.ContainsKey(dependencyType))
                {
                    this.resolvableDependencies.Add(dependencyType, autowiredValue);
                }
            }
        }

        /// <summary>
        /// Return the registered
        /// <see cref="Oragon.Spring.Objects.Factory.Config.IObjectDefinition"/> for the
        /// given object, allowing access to its property values and constructor
        /// argument values.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <returns>
        /// The registered <see cref="Oragon.Spring.Objects.Factory.Config.IObjectDefinition"/>, 
        /// or <c>null</c>, if specified object definitions does not exist.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="name"/> is <c>null</c> or empty string.
        /// </exception>
        /// <seealso cref="IConfigurableListableObjectFactory.GetObjectDefinition(string)"/>
        public override IObjectDefinition GetObjectDefinition(string name)
        {
            return GetObjectDefinition(name, false);
        }

        /// <summary>
        /// Return the registered
        /// <see cref="Oragon.Spring.Objects.Factory.Config.IObjectDefinition"/> for the
        /// given object, allowing access to its property values and constructor
        /// argument values.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <param name="includeAncestors">Whether to search parent object factories.</param>
        /// <returns>
        /// The registered <see cref="Oragon.Spring.Objects.Factory.Config.IObjectDefinition"/>, 
        /// or <c>null</c>, if specified object definitions does not exist.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="name"/> is <c>null</c> or empty string.
        /// </exception>
        /// <seealso cref="IConfigurableListableObjectFactory.GetObjectDefinition(string)"/>
        public override IObjectDefinition GetObjectDefinition(string name, bool includeAncestors)
        {
            if (StringUtils.IsNullOrEmpty(name))
            {
                throw new ArgumentException(
                    "Cannot get an object definition with a null or zero length object name string.");
            }

            name = TransformedObjectName(name);
            IObjectDefinition definition;
            if (!objectDefinitionMap.TryGetValue(name, out definition))
            {
                if (!includeAncestors || ParentObjectFactory == null)
                {
                    return null;
                }
                else if (ParentObjectFactory is AbstractObjectFactory)
                {
                    definition =
                        ((AbstractObjectFactory)ParentObjectFactory).GetObjectDefinition(name, includeAncestors);
                }
            }
            return definition;
        }

        #endregion

        #region IListableObjectFactory Members


        /// <summary>
        /// Return the names of all objects defined in this factory.
        /// </summary>
        /// <returns>
        /// The names of all objects defined in this factory, or an empty array if none
        /// are defined.  Respects any Parent-Child hierarchy the factory is participating in.
        /// </returns>
        /// <seealso cref="Oragon.Spring.Objects.Factory.IListableObjectFactory.GetObjectDefinitionNames()"/>
        public IList<string> GetObjectDefinitionNames()
        {
            return GetObjectDefinitionNames(false);
        }

        /// <summary>
        /// Return the names of all objects defined in this factory, if <code>includeAncestors</code> is <code>true</code>
        /// includes all parent factories.
        /// </summary>
        /// <param name="includeAncestors">to include parent factories in result</param>
        /// <returns>
        /// The names of all objects defined in this factory, if <code>includeAncestors</code> is <code>true</code> includes all 
        /// objects defined in parent factories, or an empty array if none are defined.
        /// </returns>
        public IList<string> GetObjectDefinitionNames(bool includeAncestors)
        {
            IList<string> results = new List<string>(objectDefinitionNames);

            var listableObjectFactory = ParentObjectFactory as IListableObjectFactory;

            if (includeAncestors && listableObjectFactory != null)
            {
                foreach (var name in listableObjectFactory.GetObjectDefinitionNames(includeAncestors))
                {
                    if (!results.Contains(name))
                    {
                        results.Add(name);    
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Return the names of objects matching the given <see cref="System.Type"/>
        /// (including subclasses), judging from the object definitions.
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type"/> (class or interface) to match, or <see langword="null"/>
        /// for all object names.
        /// </param>
        /// <returns>
        /// The names of all objects defined in this factory, or an empty array if none
        /// are defined.
        /// </returns>
        /// <seealso cref="Oragon.Spring.Objects.Factory.IListableObjectFactory.GetObjectDefinitionNames()"/>
        public IList<string> GetObjectDefinitionNames(Type type)
        {
            return GetObjectDefinitionNames(type, false);
        }

        public IList<string> GetObjectDefinitionNames(Type type, bool includeAncestor)
        {
            List<string> matches = new List<string>();
            foreach (string name in GetObjectDefinitionNames(includeAncestor))
            {
                if (IsObjectDefinitionTypeMatch(name, type, includeAncestor))
                {
                    matches.Add(name);
                }
            }
            return matches;            
        }

        /// <summary>
        /// Return the names of objects matching the given <see cref="System.Type"/>
        /// (including subclasses), judging from the object definitions.
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type"/> (class or interface) to match, or <see langword="null"/>
        /// for all object names.
        /// </param>
        /// <returns>
        /// The names of all objects defined in this factory, or an empty array if none
        /// are defined.
        /// </returns>
        /// <seealso cref="Oragon.Spring.Objects.Factory.IListableObjectFactory.GetObjectNamesForType(Type)"/>
        public IList<string> GetObjectNamesForType(Type type)
        {
            return GetObjectNamesForType(type, true, true);
        }

        /// <summary>
        /// Return the names of objects matching the given <see cref="System.Type"/>
        /// (including subclasses), judging from the object definitions.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Does consider objects created by <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s,
        /// or rather it considers the type of objects created by
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/> (which means that
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s will be instantiated).
        /// </p>
        /// <p>
        /// Does not consider any hierarchy this factory may participate in.
        /// </p>
        /// </remarks>
        /// <typeparam name="T">
        /// The <see cref="System.Type"/> (class or interface) to match, or <see langword="null"/>
        /// for all object names.
        /// </typeparam>
        /// <returns>
        /// The names of all objects defined in this factory, or an empty array if none
        /// are defined.
        /// </returns>
        public IList<string> GetObjectNames<T>()
        {
            return GetObjectNamesForType(typeof(T));
        }

        /// <summary>
        /// Return the names of objects matching the given <see cref="System.Type"/>
        /// (including subclasses), judging from the object definitions.
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type"/> (class or interface) to match, or <see langword="null"/>
        /// for all object names.
        /// </param>
        /// <param name="includePrototypes">
        /// Whether to include prototype objects too or just singletons (also applies to
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s).
        /// </param>
        /// <param name="includeFactoryObjects">
        /// Whether to include <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s too
        /// or just normal objects.
        /// </param>
        /// <returns>
        /// The names of all objects defined in this factory, or an empty array if none
        /// are defined.
        /// </returns>
        /// <seealso cref="Oragon.Spring.Objects.Factory.IListableObjectFactory.GetObjectNamesForType(Type, bool, bool)"/>
        public IList<string> GetObjectNamesForType(Type type, bool includePrototypes, bool includeFactoryObjects)
        {
            List<string> objectNames = DoGetObjectNamesForType(type, includePrototypes, includeFactoryObjects);
            return objectNames;
        }

        /// <summary>
        /// Return the names of objects matching the given <see cref="System.Type"/>
        /// (including subclasses), judging from the object definitions.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Does consider objects created by <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s,
        /// or rather it considers the type of objects created by
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/> (which means that
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s will be instantiated).
        /// </p>
        /// <p>
        /// Does not consider any hierarchy this factory may participate in.
        /// Use <see cref="ObjectFactoryUtils.ObjectNamesForTypeIncludingAncestors(Oragon.Spring.Objects.Factory.IListableObjectFactory,System.Type,bool,bool)"/>
        /// to include beans in ancestor factories too.
        /// &lt;p&gt;Note: Does &lt;i&gt;not&lt;/i&gt; ignore singleton objects that have been registered
        /// by other means than bean definitions.
        /// </p>
        /// </remarks>
        /// <typeparam name="T">
        /// The <see cref="System.Type"/> (class or interface) to match, or <see langword="null"/>
        /// for all object names.
        /// </typeparam>
        /// <param name="includePrototypes">
        /// Whether to include prototype objects too or just singletons (also applies to
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s).
        /// </param>
        /// <param name="includeFactoryObjects">
        /// Whether to include <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s too
        /// or just normal objects.
        /// </param>
        /// <returns>
        /// The names of all objects defined in this factory, or an empty array if none
        /// are defined.
        /// </returns>
        public IList<string> GetObjectNames<T>(bool includePrototypes, bool includeFactoryObjects)
        {
            return GetObjectNamesForType(typeof(T), includePrototypes, includeFactoryObjects);
        }

        /// <summary>
        /// Return the object instances that match the given object
        /// <see cref="System.Type"/> (including subclasses), judging from either object
        /// definitions or the value of
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject.ObjectType"/> in the case of
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s.
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type"/> (class or interface) to match.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.IDictionary"/> of the matching objects,
        /// containing the object names as keys and the corresponding object instances
        /// as values.
        /// </returns>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// If the objects could not be created.
        /// </exception>
        /// <seealso cref="Oragon.Spring.Objects.Factory.IListableObjectFactory.GetObjectsOfType(Type)"/>
        public IDictionary<string, object> GetObjectsOfType(Type type)
        {
            return GetObjectsOfType(type, true, true);
        }

        /// <summary>
        /// Return the object instances that match the given object
        /// <see cref="System.Type"/> (including subclasses), judging from either object
        /// definitions or the value of
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject.ObjectType"/> in the case of
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s.
        /// </summary>
        /// <remarks>
        /// <p>
        /// This version of the <see cref="IListableObjectFactory.GetObjectsOfType(Type,bool,bool)"/>
        /// method matches all kinds of object definitions, be they singletons, prototypes, or
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s. Typically, the results
        /// of this method call will be the same as a call to
        /// <code>IListableObjectFactory.GetObjectsOfType(type,true,true)</code> .
        /// </p>
        /// </remarks>
        /// <typeparam name="T">
        /// The <see cref="System.Type"/> (class or interface) to match.
        /// </typeparam>
        /// <returns>
        /// A <see cref="System.Collections.IDictionary"/> of the matching objects,
        /// containing the object names as keys and the corresponding object instances
        /// as values.
        /// </returns>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// If the objects could not be created.
        /// </exception>
        public IDictionary<string, T> GetObjects<T>()
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            DoGetObjectsOfType(typeof(T), true, true, result);
            return result;
        }

        /// <summary>
        /// Return the object instances that match the given object
        /// <see cref="System.Type"/> (including subclasses).
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type"/> (class or interface) to match.
        /// </param>
        /// <param name="includePrototypes">
        /// Whether to include prototype objects too or just singletons (also applies to
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s).
        /// </param>
        /// <param name="includeFactoryObjects">
        /// Whether to include <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s too
        /// or just normal objects.
        /// </param>
        /// <returns>
        /// An <see cref="System.Collections.IDictionary"/> of the matching objects,
        /// containing the object names as keys and the corresponding object instances
        /// as values.
        /// </returns>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// If any of the objects could not be created.
        /// </exception>
        /// <seealso cref="Oragon.Spring.Objects.Factory.IListableObjectFactory.GetObjectsOfType(Type, bool, bool)"/>
        public IDictionary<string, object> GetObjectsOfType(Type type, bool includePrototypes, bool includeFactoryObjects)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            DoGetObjectsOfType(type, includePrototypes, includeFactoryObjects, result);
            return result;
        }

        private void DoGetObjectsOfType(Type type, bool includePrototypes, bool includeFactoryObjects, IDictionary resultCollector)
        {
            IList<string> objectNames = DoGetObjectNamesForType(type, includePrototypes, includeFactoryObjects);
            foreach (string objectName in objectNames)
            {
                try
                {
                    resultCollector.Add(objectName, GetObject(objectName));
                }
                catch (ObjectCreationException ex)
                {
                    if (ex.InnerException != null
                        && ex.GetBaseException().GetType().Equals(typeof(ObjectCurrentlyInCreationException)))
                    {
                        // ignoring this is ok... it indicates a circular reference when autowiring
                        // constructors; we want to find matches other than the currently
                        // created object itself...
                        if (log.IsDebugEnabled)
                        {
                            log.Debug(string.Format(
                                CultureInfo.InvariantCulture,
                                "Ignoring match to currently created object '{0}'.",
                                objectName), ex);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Return the object instances that match the given object
        /// <see cref="System.Type"/> (including subclasses), judging from either object
        /// definitions or the value of
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject.ObjectType"/> in the case of
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s.
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="System.Type"/> (class or interface) to match.
        /// </typeparam>
        /// <param name="includePrototypes">
        ///   Whether to include prototype objects too or just singletons (also applies to
        ///   <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s).
        /// </param>
        /// <param name="includeFactoryObjects">
        ///   Whether to include <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s too
        ///   or just normal objects.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.IDictionary"/> of the matching objects,
        /// containing the object names as keys and the corresponding object instances
        /// as values.
        /// </returns>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// If the objects could not be created.
        /// </exception>
        public IDictionary<string, T> GetObjects<T>(bool includePrototypes, bool includeFactoryObjects)
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            DoGetObjectsOfType(typeof(T), includePrototypes, includeFactoryObjects, result);
            return result;
        }

        /// <summary>
        /// Return an instance (possibly shared or independent) of the given object name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method allows an object factory to be used as a replacement for the
        /// Singleton or Prototype design pattern.
        /// </para>
        /// <para>
        /// Note that callers should retain references to returned objects. There is no
        /// guarantee that this method will be implemented to be efficient. For example,
        /// it may be synchronized, or may need to run an RDBMS query.
        /// </para>
        /// <para>
        /// Will ask the parent factory if the object cannot be found in this factory
        /// instance.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <returns>The instance of the object.</returns>
        /// <exception cref="Oragon.Spring.Objects.Factory.NoSuchObjectDefinitionException">
        /// If there's no such object definition.
        /// </exception>
        /// <exception cref="Oragon.Spring.Objects.Factory.ObjectDefinitionStoreException">
        /// If there is more than a single object of the requested type defined in the factory.
        /// </exception>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// If the object could not be created.
        /// </exception>
        public override T GetObject<T>()
        {
            IList<string> objectNamesForType = GetObjectNamesForType(typeof(T));

            if (objectNamesForType.Count > 1)
            {
                IList<string> autowireCandidates = new List<string>();
                foreach (var objectName in objectNamesForType)
                {
                    if (GetObjectDefinition(objectName).IsAutowireCandidate)
                        autowireCandidates.Add(objectName);

                }
                if (autowireCandidates.Count > 0)
                    objectNamesForType = autowireCandidates;
            }

            if ((objectNamesForType == null) || (objectNamesForType.Count == 0))
            {
                throw new NoSuchObjectDefinitionException(typeof(T).FullName, "Requested Type not Defined in the Context.");
            }

            if (objectNamesForType.Count == 1)
            {
                return (T)GetObject(objectNamesForType[0]);
            }
            else if (objectNamesForType.Count == 0 && ParentObjectFactory != null)
            {
                return ParentObjectFactory.GetObject<T>();
            }
            else
            {
                throw new NoSuchObjectDefinitionException(typeof(T), "expected single bean but found " +
                        objectNamesForType.Count + ": " + StringUtils.ArrayToCommaDelimitedString(objectNamesForType));
            }
        }

        /// <summary>
        /// Return the object instances that match the given object
        /// <see cref="System.Type"/> (including subclasses).
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type"/> (class or interface) to match.
        /// </param>
        /// <param name="includeNonSingletons">
        /// Whether to include prototype objects too or just singletons (also applies to
        /// <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s).
        /// </param>
        /// <param name="allowEagerInit">
        /// Whether to include <see cref="Oragon.Spring.Objects.Factory.IFactoryObject"/>s too
        /// or just normal objects.
        /// </param>
        /// <returns>
        /// An <see cref="System.Collections.IDictionary"/> of the matching objects,
        /// containing the object names as keys and the corresponding object instances
        /// as values.
        /// </returns>
        /// <exception cref="Oragon.Spring.Objects.ObjectsException">
        /// If any of the objects could not be created.
        /// </exception>
        /// <seealso cref="Oragon.Spring.Objects.Factory.IListableObjectFactory.GetObjectsOfType(Type, bool, bool)"/>
        protected List<string> DoGetObjectNamesForType(Type type, bool includeNonSingletons, bool allowEagerInit)
        {
            List<string> result = new List<string>();
            IList<string> objectNames = GetObjectDefinitionNames(true);
            foreach (string s in objectNames)
            {
                string objectName = s;
                if (!IsAlias(objectName))
                {
                    try
                    {
                        RootObjectDefinition mod = GetMergedObjectDefinition(objectName, true);
                        // Only check object definition if it is complete
                        if (!mod.IsAbstract &&
                                (allowEagerInit || (mod.HasObjectType || !mod.IsLazyInit /*|| this.AllowEagerTypeLoading*/ ) &&
                                    !RequiresEagerInitForType(mod.FactoryObjectName)))
                        {
                            bool isFactoryObject = IsFactoryObject(objectName, mod);
                            bool matchFound =
                                   (allowEagerInit || !isFactoryObject || ContainsSingleton(objectName)) &&
                                   (includeNonSingletons || IsSingleton(objectName)) && IsTypeMatch(objectName, type);
                            if (!matchFound && isFactoryObject)
                            {
                                // in case of a FactoryObject, try to match FactoryObject instance itself next
                                objectName = ObjectFactoryUtils.BuildFactoryObjectName(objectName);
                                matchFound = (includeNonSingletons || mod.IsSingleton) && IsTypeMatch(objectName, type);
                            }
                            if (matchFound)
                            {
                                result.Add(objectName);
                            }
                        }
                    }
                    catch (CannotLoadObjectTypeException ex)
                    {
                        if (allowEagerInit)
                        {
                            throw;
                        }
                        // Probably contains a placeholder; lets ignore it for type matching purposes.
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("Ignoring object class loading failure for object '" + objectName + "'", ex);
                        }
                    }
                    catch (ObjectDefinitionStoreException ex)
                    {
                        if (allowEagerInit)
                        {
                            throw;
                        }
                        // Probably contains a placeholder; lets ignore it for type matching purposes.
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("Ignoring unresolvable metadata in object definition '" + objectName + "'", ex);
                        }
                    }
                }
            }

            // check singletons too, to catch manually registered singletons...
            IList<string> singletonNames = GetSingletonNames();
            foreach (string s in singletonNames)
            {
                string objectName = s;
                // only check if manually registered...
                if (!ContainsObjectDefinition(objectName))
                {
                    // in the case of an IFactoryObject, match the object created by the IFactoryObject...
                    if (IsFactoryObject(objectName))
                    {
                        if ((includeNonSingletons || IsSingleton(objectName)) && IsTypeMatch(objectName, type))
                        {
                            result.Add(objectName);
                            continue;
                        }
                        objectName = ObjectFactoryUtils.BuildFactoryObjectName(objectName);
                    }
                    if (IsTypeMatch(objectName, type))
                    {
                        result.Add(objectName);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Check whether the specified bean would need to be eagerly initialized
        /// in order to determine its type.
        /// </summary>
        /// <param name="factoryObjectName">a factory-bean reference that the bean definition defines a factory method for</param>
        /// <returns>whether eager initialization is necessary</returns>
        private bool RequiresEagerInitForType(String factoryObjectName)
        {
            return (factoryObjectName != null && IsFactoryObject(factoryObjectName) && !ContainsSingleton(factoryObjectName));
        }

        /// <summary>
        /// Check whether the given bean is defined as a <see cref="IFactoryObject"/>. 
        /// </summary>
        /// <param name="objectName">the name of the object</param>
        /// <param name="rod">the corresponding object definition</param>
        protected bool IsFactoryObject(String objectName, RootObjectDefinition rod)
        {
            Type objectType = PredictObjectType(objectName, rod);
            return (objectType != null && typeof(IFactoryObject).IsAssignableFrom(objectType));
        }

        #endregion

        /// <summary>
        /// Resolve the specified dependency against the objects defined in this factory.
        /// </summary>
        /// <param name="descriptor">The descriptor for the dependency.</param>
        /// <param name="objectName">Name of the object which declares the present dependency.</param>
        /// <param name="autowiredObjectNames">A list that all names of autowired object (used for
        /// resolving the present dependency) are supposed to be added to.</param>
        /// <returns>
        /// the resolved object, or <code>null</code> if none found
        /// </returns>
        /// <exception cref="ObjectsException">if dependency resolution failed</exception>
        public override object ResolveDependency(DependencyDescriptor descriptor, string objectName,
                                                 IList autowiredObjectNames)
        {
            Type type = descriptor.DependencyType;
            Object value = AutowireCandidateResolver.GetSuggestedValue(descriptor);
		    if (value != null)
            {
			    if (value is string)
			    {
			        object valueBefore = value;
			        value = ResolveEmbeddedValue((string) value);
                    if (valueBefore.Equals(value))
			            value = ExpressionEvaluator.GetValue(null, (string) value);
			    }
                return TypeConversionUtils.ConvertValueIfNecessary(type, value, null);
		    }

            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                IDictionary matchingObjects = FindAutowireCandidates(objectName, elementType, descriptor);
                if (matchingObjects.Count == 0)
                {
                    if (descriptor.Required)
                    {
                        RaiseNoSuchObjectDefinitionException(elementType, "array of " + elementType.FullName, descriptor);
                    }
                    return null;
                }
                if (autowiredObjectNames != null)
                {
                    foreach (DictionaryEntry matchingObject in matchingObjects)
                    {
                        autowiredObjectNames.Add(matchingObject.Key);
                    }
                }
                return TypeConversionUtils.ConvertValueIfNecessary(type, matchingObjects.Values, null);
            }
            else if (type.IsGenericType && 
                (type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetGenericTypeDefinition() == typeof(Oragon.Spring.Collections.Generic.ISet<>) || 
                 type.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
            {
                var isDictionary = (type.GetGenericTypeDefinition() == typeof (IDictionary<,>));
                var elementType = isDictionary ? type.GetGenericArguments()[1] : type.GetGenericArguments()[0];

                if (isDictionary && type.GetGenericArguments()[0] != typeof(string))
                    throw new NoSuchObjectDefinitionException(type,
                                    "expected first generic to be a string but is " + type.GetGenericArguments()[0]);

                IDictionary matchingObjects = FindAutowireCandidates(objectName, elementType, descriptor);
                if (matchingObjects.Count == 0)
                {
                    if (descriptor.Required)
                    {
                        RaiseNoSuchObjectDefinitionException(elementType, "dictionary/list/set of " + elementType.FullName, descriptor);
                    }
                    return null;
                }
                if (autowiredObjectNames != null)
                {
                    foreach (DictionaryEntry matchingObject in matchingObjects)
                    {
                        autowiredObjectNames.Add(matchingObject.Key);
                    }
                }

                return isDictionary
                           ? TypeConversionUtils.ConvertValueIfNecessary(type, matchingObjects, null)
                           : TypeConversionUtils.ConvertValueIfNecessary(type, matchingObjects.Values, null);
            }
            else if (typeof(ICollection).IsAssignableFrom(type) && type.IsInterface)
            {
                //TODO - handle generic types.
                return null;

            }
            else
            {
                IDictionary matchingObjects = FindAutowireCandidates(objectName, type, descriptor);
                if (matchingObjects.Count == 0)
                {
                    if (descriptor.Required)
                    {
                        string methodType = (descriptor.MethodParameter.ConstructorInfo != null) ? "constructor" : "method";
                        throw new NoSuchObjectDefinitionException(type,
                             "Unsatisfied dependency of type [" + type + "]: expected at least 1 matching object to wire the ["
                             + descriptor.MethodParameter.ParameterName() + "] parameter on the " + methodType + " of object [" + objectName + "]");
                    }
                    return null;
                }
                if (matchingObjects.Count > 1)
                {
                    string primaryObjecName = DeterminePrimaryCandidate(matchingObjects, descriptor);
                    if (primaryObjecName == null)
                    {
                        throw new NoSuchObjectDefinitionException(type,
                                        "expected single matching object but found " + matchingObjects.Count + ": " + matchingObjects);
                    }
                    if (autowiredObjectNames != null)
                    {
                        autowiredObjectNames.Add(primaryObjecName);
                    }
                    return matchingObjects[primaryObjecName];
                }
                DictionaryEntry entry = (DictionaryEntry)ObjectUtils.EnumerateFirstElement(matchingObjects);
                if (autowiredObjectNames != null)
                {
                    autowiredObjectNames.Add(entry.Key);
                }
                return entry.Value;
            }
        }

        /// <summary>
        /// Determine the primary autowire candidate in the given set of beans.
        /// </summary>
        /// <param name="candidateObjects">a Map of candidate names and candidate instances
        /// that match the required type</param>
        /// <param name="descriptor">the target dependency to match against</param>
        /// <returns>the name of the primary candidate, or <code>null</code> if none found</returns>
        private string DeterminePrimaryCandidate(IDictionary candidateObjects, DependencyDescriptor descriptor) {
		    string primaryObjectName = null;
		    string fallbackObjectName = null;
		    foreach(DictionaryEntry entry in candidateObjects)
		    {
		        string candidateBeanName = entry.Key as string;
			    object objectInstance = entry.Value;
			    if (IsPrimary(candidateBeanName, objectInstance))
                {
				    if (primaryObjectName != null) 
                    {
					    bool candidateLocal = ContainsObjectDefinition(candidateBeanName);
					    bool primaryLocal = ContainsObjectDefinition(primaryObjectName);
					    if (candidateLocal == primaryLocal)
                        {
						    throw new NoSuchObjectDefinitionException(descriptor.DependencyType,
								    "more than one 'primary' bean found among candidates: " + candidateObjects);
					    }
					    if (candidateLocal && !primaryLocal)
                        {
						    primaryObjectName = candidateBeanName;
					    }
				    }
				    else
                    {
					    primaryObjectName = candidateBeanName;
				    }
			    }
			    if (primaryObjectName == null &&
                        (resolvableDependencies.Values.Contains(objectInstance) ||
							    MatchesObjectName(candidateBeanName, descriptor.DependencyName)))
                {
				    fallbackObjectName = candidateBeanName;
			    }
		    }
		    return (primaryObjectName ?? fallbackObjectName);
	    }

        /// <summary>
        /// Return whether the object definition for the given object name has been
        /// marked as a primary object.
        /// </summary>
        /// <param name="objectName">the name of the bean</param>
        /// <param name="objectInstance">the corresponding bean instance</param>
        /// <returns>whether the given bean qualifies as primary</returns>
        private bool IsPrimary(string objectName, object objectInstance) {
		    if (ContainsObjectDefinition(objectName)) {
			    return GetMergedObjectDefinition(objectName, true).IsPrimary;
		    }
		    return (ParentObjectFactory is DefaultListableObjectFactory &&
                    ((DefaultListableObjectFactory)ParentObjectFactory).IsPrimary(objectName, objectInstance));
	    }

        /// <summary>
        /// Determine whether the given candidate name matches the bean name or the aliases
        ///stored in this bean definition.
        /// </summary>
        protected bool MatchesObjectName(string objectName, string candidateName)
        {
            return (candidateName != null &&
                    (candidateName.Equals(objectName) || GetAliases(objectName).Contains(candidateName)));
        }

        /// <summary>
        /// Raises the no such object definition exception for an unresolvable dependency
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="dependencyDescription">The dependency description.</param>
        /// <param name="descriptor">The descriptor.</param>
        private void RaiseNoSuchObjectDefinitionException(Type type, string dependencyDescription, DependencyDescriptor descriptor)
        {
            throw new NoSuchObjectDefinitionException(type, dependencyDescription,
                                                      "expected at least 1 object which qualifies as autowire candidate for this dependency. ");
        }

        private IDictionary FindAutowireCandidates(string objectName, Type requiredType, DependencyDescriptor descriptor)
        {
            IList<string> candidateNames =
                ObjectFactoryUtils.ObjectNamesForTypeIncludingAncestors(this, requiredType, true, descriptor.Eager);
            IDictionary result = new OrderedDictionary(candidateNames.Count);

            foreach (var entry in resolvableDependencies)
            {
                Type autoWiringType = entry.Key;
                if (autoWiringType.IsAssignableFrom(requiredType))
                {
                    object autowiringValue = this.resolvableDependencies[autoWiringType];
                    if (requiredType.IsInstanceOfType(autowiringValue))
                    {
                        result.Add(ObjectUtils.IdentityToString(autowiringValue), autowiringValue);
                        break;
                    }
                }
            }

            for (int i = 0; i < candidateNames.Count; i++)
            {
                string candidateName = candidateNames[i];
                if (!candidateName.Equals(objectName) && IsAutowireCandidate(candidateName, descriptor))
                {
                    result.Add(candidateName, GetObject(candidateName));
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether the specified object qualifies as an autowire candidate,
        /// to be injected into other beans which declare a dependency of matching type.
        /// This method checks ancestor factories as well.
        /// </summary>
        /// <param name="objectName">Name of the object to check.</param>
        /// <param name="descriptor">The descriptor of the dependency to resolve.</param>
        /// <returns>
        /// 	<c>true</c> if the object should be considered as an autowire candidate; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NoSuchObjectDefinitionException">if there is no object with the given name.</exception>
        public bool IsAutowireCandidate(string objectName, DependencyDescriptor descriptor)
        {
            //Consider FactoryObjects as autowiring candidates.
            bool isFactoryObject = (descriptor != null && descriptor.DependencyType != null &&
                                    typeof(IFactoryObject).IsAssignableFrom(descriptor.DependencyType));
            if (isFactoryObject)
            {
                objectName = ObjectFactoryUtils.TransformedObjectName(objectName);
            }

            if (!ContainsObjectDefinition(objectName))
            {
                if (ContainsSingleton(objectName))
                {
                    return true;
                }
                else if (ParentObjectFactory is IConfigurableListableObjectFactory)
                {
                    // No object definition found in this factory -> delegate to parent
                    return
                        ((IConfigurableListableObjectFactory)ParentObjectFactory).IsAutowireCandidate(objectName, descriptor);
                }
            }
            return IsAutowireCandidate(objectName, GetMergedObjectDefinition(objectName, true), descriptor);
        }

        /// <summary>
        /// Determine whether the specified object definition qualifies as an autowire candidate,
        /// to be injected into other beans which declare a dependency of matching type.
        /// </summary>
        /// <param name="objectName">Name of the object definition to check.</param>
        /// <param name="rod">The merged object definiton to check.</param>
        /// <param name="descriptor">The descriptor of the dependency to resolve.</param>
        /// <returns>
        /// 	<c>true</c> if the object should be considered as an autowire candidate; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAutowireCandidate(string objectName, RootObjectDefinition rod, DependencyDescriptor descriptor)
        {
            ResolveObjectType(rod, objectName);
            return
                AutowireCandidateResolver.IsAutowireCandidate(
                    new ObjectDefinitionHolder(rod, objectName, GetAliases(objectName)), descriptor);
        }
    }
}

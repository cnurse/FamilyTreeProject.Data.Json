using System;
using System.Collections.Generic;
using System.Linq;
using FamilyTreeProject.Core;
using FamilyTreeProject.Core.Common;
using FamilyTreeProject.Data.Common;
using FamilyTreeProject.Data.Common.DataModels;
using FamilyTreeProject.Data.Common.Mapping;

namespace FamilyTreeProject.Data.Json
{
    public class JsonFileStore : FileStore
    {
        public JsonFileStore(string path) : base(path, new JsonDocument()) {}

        private JsonDocument JsonDocument => Document as JsonDocument;

        protected override void LoadFamilies()
        {
            Families = JsonDocument.Families.ToModel();
        }

        protected override void LoadIndividuals()
        {
            Individuals = JsonDocument.Individuals.ToModel();
        }

        protected override void LoadRepositories()
        {
            Repositories = JsonDocument.Repositories.ToModel();
        }

        protected override void LoadSources()
        {
            Sources = JsonDocument.Sources.ToModel();
        }

        protected override void LoadTree()
        {
            Tree = JsonDocument.Tree.ToModel();
        }

        public override void DeleteFamily(Family family)
        {
            throw new NotImplementedException();
        }

        public override void DeleteIndividual(Individual individual)
        {
            throw new NotImplementedException();
        }

        public override void DeleteRepository(Repository repository)
        {
            //Repositories.Remove(Repositories.First());
        }

        public override void DeleteSource(Source source)
        {
            throw new NotImplementedException();
        }

        public override void DeleteTree(Tree tree)
        {
            throw new NotImplementedException();
        }

        public override void SaveChanges()
        {
            
            //Process Tree
            JsonDocument.Tree = Tree.ToDataModel(false);
            
            //Processs Repositories
            foreach (var repository in Repositories)
            {
                SaveRepository(repository);
            }
            
            //Process Sources
            foreach (var source in Sources)
            {
                SaveSource(source);
            }
            
            //ProcessIndividuals
            foreach (var individual in Individuals)
            {
                SaveIndividual(individual);
            }
            
            //ProcessFamilies
            foreach (var family in Families)
            {
                SaveFamily(family);
            }

            base.SaveChanges();
        }

        public override void UpdateFamily(Family family)
        {
            throw new NotImplementedException();
        }

        public override void UpdateRepository(Repository repository)
        {
            //var repo = Repositories.First(r => r.Id == repository.Id);
            //repo = repository;
        }

        public override void UpdateSource(Source source)
        {
            throw new NotImplementedException();
        }

        public override void UpdateTree(Tree tree)
        {
            throw new NotImplementedException();
        }
        
        private void SaveEntity<TEntity, TModel>(TEntity entity, TModel dataModel, IList<TModel> collection, Action<TModel> updateAction) where TEntity : IUniqueEntity where TModel : UniqueModel
        {
            var model = collection.SingleOrDefault(e => e.UniqueId == entity.UniqueId);
            if (model != null)
            {
                updateAction(model);
            }
            else
            {
                collection.Add(dataModel);
            }
        }

        private void SaveFamily(Family family)
        {
            var dataModel = family.ToDataModel(false);

            SaveEntity(family, dataModel, JsonDocument.Families, (m) =>
            {
                m.Husband = dataModel.Husband;
                m.Wife = dataModel.Wife;
            });
        }

        private void SaveIndividual(Individual individual)
        {
            var dataModel = individual.ToDataModel(false);

            SaveEntity(individual, dataModel, JsonDocument.Individuals, (m) =>
            {
                m.FirstName = dataModel.FirstName;
                m.LastName = dataModel.LastName;
                m.Father = dataModel.Father;
                m.Mother = dataModel.Mother;
                m.Sex = dataModel.Sex;
            });
        }

        private void SaveRepository(Repository repository)
        {
            var dataModel = repository.ToDataModel(false);
            
            SaveEntity(repository, dataModel, JsonDocument.Repositories, (m) =>
            {
                m.Address = dataModel.Address;
                m.Name = dataModel.Name;
            });
        }

        private void SaveSource(Source source)
        {
            var dataModel = source.ToDataModel(false);

            SaveEntity(source, dataModel, JsonDocument.Sources, (m) =>
            {
                m.Author = dataModel.Author;
                m.Publisher = dataModel.Publisher;
                m.Repository = dataModel.Repository;
                m.Title = dataModel.Title;
            });
        }
    }

}
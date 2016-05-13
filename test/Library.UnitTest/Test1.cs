using Library.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Library.UnitTest
{
    public class Test1
    {
        public DatabaseTest DatabaseTest { get; private set; }

        [Fact]
        public void AddMaterialToCatalogTest()
        {
            User userValid = null;
            Catalog catalogValid = null;
            var material = DatabaseTest.GetOneValidMaterial();

            TestService.Config(@"USE CASE 1: Adicionar material ao catalogo da biblioteca.",
                new[]
                {
                    @"Apenas usuários admin podem adicionar materiais ao catalogo",
                    @"É necessário que todos os dados do material sejam preenchidos corretamente",
                    @"Usuarios que ja sugeriram este material recebem uma notificação"
                })
                .Done(() =>
                {
                    var users = DatabaseTest.GetUsersOfEachType();
                    foreach (var u in users)
                    {
                        var catalog = new Catalog(u);
                        if (u.TypeIs(UserType.Admin))
                        {
                            TestService.AssertFalse(catalog.IsReadonly());
                            TestService.AssertDone(catalog.AddMaterial(material));
                            userValid = u;
                            catalogValid = catalog;
                        }
                        else
                        {
                            TestService.AssertTrue(catalog.IsReadonly());
                            TestService.AssertFail(catalog.AddMaterial(material));
                        }
                    }
                })
                .Done(() =>
                {
                    var materials = DatabaseTest.GetAllInvalidMaterials();
                    foreach (var m in materials)
                    {
                        TestService.AssertFalse(m.IsValid());
                        TestService.AssertFail(catalogValid.AddMaterial(m));
                    }
                    
                    TestService.AssertDone(catalogValid.AddMaterial(material));
                })
                .Done(() =>
                {
                    material = DatabaseTest.GetOneValidMaterialSuggestedBy(userValid);
                    var notifications = new NotificationService(userValid);
                    int count = notifications.Count();

                    TestService.AssertDone(catalogValid.AddMaterial(material));
                    TestService.AssertEquals(count + 1, notifications.Count());
                });

            TestService.Finish();

        }

        /*
         * USE CASE 2: Remover material do catalogo da biblioteca
         */
        [Fact]
        public void RemoveMaterialFromCatalogTest()
        {
            User userValid = null;
            Catalog catalogValid = null;
            var material = DatabaseTest.GetOneValidMaterial();

            TestService.Config(@"USE CASE 2: Remover material do catalogo da biblioteca.",
                new[]
                {
                    @"Apenas usuários admin podem remover materiais ao catalogo",
                    @"É necessário que o material exista, não esteja emprestado nem tenha sido reservado"
                })
                .Done(() =>
                {
                    var users = DatabaseTest.GetUsersOfEachType();
                    foreach (var u in users)
                    {
                        var catalog = new Catalog(u);
                        if (u.TypeIs(UserType.Admin))
                        {
                            TestService.AssertFalse(catalog.IsReadonly());
                            TestService.AssertDone(catalog.RemoveMaterial(material));
                            userValid = u;
                            catalogValid = catalog;
                        }
                        else
                        {
                            TestService.AssertTrue(catalog.IsReadonly());
                            TestService.AssertFail(catalog.RemoveMaterial(material));
                        }
                    }
                })
                .Done(() =>
                {
                    TestService.AssertFail(catalogValid.RemoveMaterial(null));
                    TestService.AssertFail(catalogValid.RemoveMaterial(new Material()));

                    var m1 = DatabaseTest.GetOneReservedMaterial();
                    TestService.AssertFail(catalogValid.RemoveMaterial(m1));

                    var m2 = DatabaseTest.GetOneBorrowedMaterial();
                    TestService.AssertFail(catalogValid.RemoveMaterial(m2));
                    
                    TestService.AssertDone(catalogValid.AddMaterial(material));
                });

            TestService.Finish();
        }

        /*
         * USE CASE 3: Sugerir material ao catalogo da biblioteca
         */
        [Fact]
        public void SuggestMaterialToCatalogTest()
        {

        }

        /*USE CASE 4: Procurar por material no catalogo da biblioteca
         */
        [Fact]
        public void SearchMaterialInCatalogTest()
        {

        }

        /*
         * USE CASE 5: Cadastrar empregado da biblioteca
         */
        [Fact]
        public void CreateStaffTest()
        {

        }

        /*
         * USE CASE 6: Remover empregado da biblioteca
         */
        [Fact]
        public void DeleteStaffTest()
        {

        }

        /*
         * USE CASE 7: Atualizar dados do empregado da biblioteca
         */
        [Fact]
        public void UpdateStaffTest()
        {

        }

        /*
         * USE CASE 8: Cadastrar usuario da biblioteca
         */
        [Fact]
        public void RegisterUserTest()
        {

        }

        /*
         * USE CASE 9: Remover usuario da biblioteca
         */
        public void UnregisterUserTest()
        {

        }

        /*
         * USE CASE 10: Atualizar dados do usuario da biblioteca
         */
        public void UpdateUserTest()
        {

        }

        /*
         * USE CASE 11: Empréstimos de materiais
         */
        public void BorrowingMaterialsTest()
        {

        }

        /*
         * USE CASE 12: Empréstimos de materiais exclusivos
         */
        public void BorrowingExclusiveMaterialsTest()
        {

        }

        /*
         * USE CASE 13: Devolução de materiais
         */
        public void ReturnMaterialsTest()
        {

        }

        /*
         * USE CASE 14: Renovação de materiais
         */
        public void RenewMaterialsTest()
        {

        }

        /*
         * USE CASE 15: Solicitação de recebimento de materiais em domicilio
         */
        public void RequestMaterialsTest()
        {

        }

        /*
         * USE CASE 16: Envio de materiais pelo correio
         */
        public void PostRequestedMaterialsTest()
        {

        }

        /*
         * USE CASE 17: Devolução de materiais pelo correio
         */
        public void ReceiveReturnRequestedMaterialTest()
        {

        }
    }
}

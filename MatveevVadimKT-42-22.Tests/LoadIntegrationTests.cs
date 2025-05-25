using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Interfaces.LoadInterfaces;
using MatveevVadimKt_42_22.Models;
using MatveevVadimKt_42_22.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatveevVadimKT_42_22.Tests
{
    
        public class LoadIntegrationTests
        {
            private readonly DbContextOptions<UniversityDbContext> _dbContextOptions;

            public LoadIntegrationTests()
            {
                _dbContextOptions = new DbContextOptionsBuilder<UniversityDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Используем временную базу данных для тестов
                    .Options;
            }

            // 1. Тест на создание нагрузки
            [Fact]
            public async Task AddLoad_ShouldReturnLoad_WhenLoadIsAdded()
            {
                // Arrange
                var context = new UniversityDbContext(_dbContextOptions);
                var loadService = new LoadService(context);

                // Создаем DTO для добавления нагрузки
                var addLoadDto = new AddLoadDto
                {
                    TeacherId = 1,
                    DisciplineId = 1,
                    Hours = 20
                };

                // Act
                var savedLoad = await loadService.AddLoadAsync(addLoadDto, CancellationToken.None);

                // Assert
                Assert.NotNull(savedLoad);  // Проверяем, что нагрузка была сохранена
                Assert.Equal(20, savedLoad.Hours);  // Проверяем, что количество часов сохранено правильно
            }

            // 2. Тест на извлечение нагрузки по ID
            [Fact]
            public async Task GetLoadById_ShouldReturnCorrectLoad_WhenLoadExists()
            {
                // Arrange
                var context = new UniversityDbContext(_dbContextOptions);
                var loadService = new LoadService(context);

                var load = new Load
                {
                    TeacherId = 1,
                    DisciplineId = 1,
                    Hours = 20
                };
                await context.AddAsync(load);
                await context.SaveChangesAsync();

                // Act
                var retrievedLoad = await loadService.GetLoadByIdAsync(load.Id, CancellationToken.None);

                // Assert
                Assert.NotNull(retrievedLoad);  // Проверяем, что нагрузка была извлечена
                Assert.Equal(20, retrievedLoad.Hours);  // Проверяем, что количество часов соответствует сохраненному значению
            }

            // 3. Тест на обновление нагрузки
            [Fact]
            public async Task UpdateLoad_ShouldUpdateLoad_WhenLoadExists()
            {
                // Arrange
                var context = new UniversityDbContext(_dbContextOptions);
                var loadService = new LoadService(context);

                var load = new Load
                {
                    TeacherId = 1,
                    DisciplineId = 1,
                    Hours = 20
                };
                await context.AddAsync(load);
                await context.SaveChangesAsync();

                load.Hours = 25;  // Обновляем количество часов

                // Act
                var updatedLoad = await loadService.UpdateLoadAsync(new UpdateLoadDto
                {
                    Id = load.Id,
                    TeacherId = load.TeacherId,
                    DisciplineId = load.DisciplineId,
                    Hours = load.Hours
                }, CancellationToken.None);

                // Assert
                Assert.NotNull(updatedLoad);  // Проверяем, что нагрузка была обновлена
                Assert.Equal(25, updatedLoad.Hours);  // Проверяем, что количество часов обновилось
            }

            // 4. Тест на удаление нагрузки
            [Fact]
            public async Task DeleteLoad_ShouldDeleteLoad_WhenLoadExists()
            {
                // Arrange
                var context = new UniversityDbContext(_dbContextOptions);
                var loadService = new LoadService(context);

                var load = new Load
                {
                    TeacherId = 1,
                    DisciplineId = 1,
                    Hours = 20
                };
                await context.AddAsync(load);
                await context.SaveChangesAsync();

                // Act
                var result = await loadService.DeleteLoadAsync(load.Id, CancellationToken.None);

                // Assert
                Assert.True(result);  // Проверяем, что удаление прошло успешно
                var deletedLoad = await context.Loads.FindAsync(load.Id);
                Assert.Null(deletedLoad);  // Проверяем, что нагрузка была действительно удалена
            }
        }
    }


using LabWork20.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabWork20.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private static readonly List<Cat> _cats =
        [
            new(Id: 1, Color: "Рыжий", Breed: "Дворовая", Name: "Барсик", Age: 3),
            new(Id: 2, Color: "Черный", Breed: "Британская", Name: "Кнопка", Age: 2),
            new(Id: 3, Color: "Белый", Breed: "Персидская", Name: "Снежок", Age: 5),
            new(Id: 4, Color: "Серый", Breed: "Мейн-кун", Name: "Граф", Age: 7),
            new(Id: 5, Color: "Трехцветная", Breed: "Сиамская", Name: "Муся", Age: 4)
        ];

        [HttpGet]
        public ActionResult<IEnumerable<Cat>> GetAll(int? page = null, int? pageSize = null)
        {
            if ((page.HasValue && page < 1) || (pageSize.HasValue && pageSize < 1))
                return BadRequest("Page and pagesize must be >= 1");

            IEnumerable<Cat> result = _cats;

            if (page.HasValue && pageSize.HasValue)
                result = result.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return Ok(result.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (!_cats.Any(c => c.Id == id))
            {
                return NotFound($"Кот с id {id} не найден");
            }

            var cat = _cats.First(c => c.Id == id);
            return Ok(cat);
        }

        [HttpGet("breed/{breed}")]
        public IActionResult GetByBreed(string breed)
        {
            var catsByBreed = _cats.Where(c =>
                c.Breed.Contains(breed, StringComparison.OrdinalIgnoreCase)).ToList();

            if (catsByBreed.Count == 0)
                return NotFound($"Коты с породой '{breed}' не найдены");

            return Ok(catsByBreed);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cat cat)
        {
            if (string.IsNullOrWhiteSpace(cat.Name))
                return BadRequest("Имя кота не может быть пустым значением");

            if (string.IsNullOrWhiteSpace(cat.Breed))
                return BadRequest("Порода кота не может быть пустым значением");

            if (string.IsNullOrWhiteSpace(cat.Color))
                return BadRequest("Цвет кота не может быть пустым значением");

            if (cat.Age < 0)
                return BadRequest("Возраст кота не может быть отрицательным");

            if (_cats.Any(c => c.Id == cat.Id))
                return Conflict($"Кот с id {cat.Id} уже существует");

            if (cat.Id < 0)
                cat.Id = _cats.Max(c => c.Id) + 1;

            _cats.Add(cat);

            return CreatedAtAction(nameof(GetById), new { id = cat.Id }, cat);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Cat updatedCat)
        {
            if (string.IsNullOrWhiteSpace(updatedCat.Name))
                return BadRequest("Имя кота обязательно");

            if (string.IsNullOrWhiteSpace(updatedCat.Breed))
                return BadRequest("Порода кота обязательна");

            if (string.IsNullOrWhiteSpace(updatedCat.Color))
                return BadRequest("Цвет кота обязателен");

            if (updatedCat.Age <= 0)
                return BadRequest("Возраст должен быть положительным числом");

            if (!_cats.Any(c => c.Id == id))
                return NotFound($"Кот с id {id} не найден");

            var index = _cats.FindIndex(c => c.Id == id);
            var newCat = updatedCat with { Id = id };
            _cats[index] = newCat;

            return Ok(newCat);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_cats.Any(c => c.Id == id))
                return NotFound($"Кот с id {id} не найден");

            var cat = _cats.First(c => c.Id == id);
            _cats.Remove(cat);

            return NoContent();
        }
    }
}
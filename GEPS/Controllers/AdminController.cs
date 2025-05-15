using GEPS.Filter;
using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace GEPS.Controllers
{
    [RoleFilter("Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ******************** Professor Area ********************

        [HttpGet("PostCreateProfessor")]
        public IActionResult PostCreateProfessor()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            return View();
        }

        [HttpPost("PostCreateProfessor")]
        public async Task<IActionResult> PostCreateProfessor([FromBody] Professor professor)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Admin/create-professor";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(professor), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = errorContent });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("GetAllProfessor")]
        public async Task<IActionResult> GetAllProfessor()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string  apiUrl = "https://localhost:7107/api/Admin/get-all-professor";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var professorList = JsonConvert.DeserializeObject<List<Professor>>(content);

                    return View(professorList);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Profesör bilgileri alınamadı. API Hatası: {response.StatusCode} - {errorContent}";
                    return View(new List<Professor>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View(new List<Professor>());
            }
        }

        [HttpPut("UpdateProfessor/{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, [FromBody] Professor professor)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/update-professor-by-id/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(professor), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = $"API call failed with status: {response.StatusCode}, {errorContent}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpGet("UpdateProfessor/{id}")]
        public async Task<IActionResult> UpdateProfessor(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/get-professor-by-id/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var professor = JsonConvert.DeserializeObject<Professor>(content);

                    return View(professor);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Profesör bilgileri alınırken bir hata oluştu: {errorMessage}";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPost("DeleteProfessor")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/delete-professor-by-id/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Profesör başarıyla silindi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Profesör silinirken bir hata oluştu.";
            }

            return RedirectToAction("GetAllProfessor");
        }

        // ******************** Student Area ********************

        [HttpGet("CreateStudent")]
        public IActionResult CreateStudent()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            return View();
        }

        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Admin/create-student";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = errorContent });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("GetAllStudent")]
        public async Task<IActionResult> GetAllStudent()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Admin/get-all-student";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var studentList = JsonConvert.DeserializeObject<List<Student>>(content);

                    return View(studentList);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Öğrenci bilgileri alınamadı. API Hatası: {response.StatusCode} - {errorContent}";
                    return View(new List<Student>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View(new List<Student>());
            }
        }

        [HttpGet("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudent(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/get-student-by-id/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var student = JsonConvert.DeserializeObject<Student>(content);

                    return View(student);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Öğrenci bilgileri alınamadı. API Hatası: {response.StatusCode} - {errorContent}";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpPost("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/update-student/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                if (id != student.UserId)
                {
                    return Json(new { success = false, errorMessage = "Student ID mismatch." });
                }

                var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = errorContent });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            var apiUrl = $"https://localhost:7107/api/Admin/delete-student/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Değerlendirme kriteri başarıyla silindi!";
                return RedirectToAction("GetAllEvaluationCriteria");
            }
            else
            {
                ViewBag.ErrorMessage = "Değerlendirme kriteri silinirken bir hata oluştu.";
                return RedirectToAction("GetAllEvaluationCriteria");
            }
        }

        // ******************** Evaluation Criteria Area ********************

        [HttpGet("GetAllEvaluationCriteria")]
        public async Task<IActionResult> GetAllEvaluationCriteria()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Admin/get-all-evaluation-criteria";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var evaluationCriteriaList = JsonConvert.DeserializeObject<List<AdminEvaluationCriteria>>(content);

                    return View(evaluationCriteriaList);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Değerlendirme kriterleri alınamadı. API Hatası: {response.StatusCode} - {errorContent}";
                    return View(new List<AdminEvaluationCriteria>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View(new List<AdminEvaluationCriteria>());
            }
        }

        [HttpGet("CreateEvaluationCriteria")]
        public IActionResult CreateEvaluationCriteria()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            return View();
        }
        
        [HttpPost("CreateEvaluationCriteria")]
        public async Task<IActionResult> CreateEvaluationCriteria([FromBody] AdminEvaluationCriteria criteria)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Admin/post-add-evaluation-criteria";

            var token = HttpContext.Session.GetString("BearerToken");
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(criteria), Encoding.UTF8, "application/json")
                };
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = errorContent });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"An unexpected error occurred: {ex.Message}" });
            }
        }

        [HttpGet("UpdateEvaluationCriteria/{id}")]
        public async Task<IActionResult> UpdateEvaluationCriteria(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/get-evaluation-criteria-by-id/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var evaluationCriteria = JsonConvert.DeserializeObject<AdminEvaluationCriteria>(content);

                    return View(evaluationCriteria);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Evaluation criteria could not be retrieved: {errorMessage}";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPost("UpdateEvaluationCriteria/{id}")]
        public async Task<IActionResult> UpdateEvaluationCriteria(int id, [FromBody] AdminEvaluationCriteria criteria)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/put-update-evaluation-criteria/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                criteria.CriteriaId = id;
                var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl) 
                {
                    Content = new StringContent(JsonConvert.SerializeObject(criteria), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = errorContent });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("DeleteEvaluationCriteria/{id}")]
        public async Task<IActionResult> DeleteEvaluationCriteria(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;
            
            var apiUrl = $"https://localhost:7107/api/Admin/delete-evaluation-criteria/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Değerlendirme kriteri başarıyla silindi!";
                return RedirectToAction("GetAllEvaluationCriteria");
            }
            else
            {
                ViewBag.ErrorMessage = "Değerlendirme kriteri silinirken bir hata oluştu.";
                return RedirectToAction("GetAllEvaluationCriteria");
            }
        }

        // ******************** Checklist Items Area ********************

        [HttpGet("GetAllChecklistItems")]
        public async Task<IActionResult> GetAllChecklistItems()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Admin/get-all-checklist-items";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var checklistItems = JsonConvert.DeserializeObject<List<AdminChecklistItem>>(content);

                    return View(checklistItems);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Checklist öğeleri alınamadı. API Hatası: {response.StatusCode} - {errorContent}";
                    return View(new List<AdminChecklistItem>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View(new List<AdminChecklistItem>());
            }
        }

        [HttpGet("PostAddCheckListItem")]
        public IActionResult PostAddCheckListItem()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            return View();
        }

        [HttpPost("PostAddCheckListItem")]
        public async Task<IActionResult> PostAddCheckListItem([FromBody] AdminChecklistItem checklistItem)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Admin/post-add-checklist-item";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(checklistItem), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = errorContent });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("UpdateCheckListItem/{id}")]
        public async Task<IActionResult> UpdateCheckListItem(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/get-checklist-item-by-id/{id}";
            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var checklistItem = JsonConvert.DeserializeObject<AdminChecklistItem>(content);

                    return View(checklistItem);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Error while fetching checklist item details: {errorMessage}";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPut("UpdateCheckListItem/{id}")]
        public async Task<IActionResult> UpdateCheckListItem(int id, [FromBody] AdminChecklistItem checklistItem)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Admin/put-update-checklist-item/{id}";
            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(checklistItem), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = errorContent });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errorMessage = $"An error occurred: {ex.Message}, InnerException: {ex.InnerException?.Message}"
                });
            }
        }

        [HttpDelete("DeleteCheckListItem/{id}")]
        public async Task<IActionResult> DeleteCheckListItem(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            var apiUrl = $"https://localhost:7107/api/Admin/delete-checklist-item/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Delete Check List Item başarıyla silindi!";
                return RedirectToAction("GetAllEvaluationCriteria");
            }
            else
            {
                ViewBag.ErrorMessage = "Delete Check List Item silinirken bir hata oluştu.";
                return RedirectToAction("GetAllEvaluationCriteria");
            }
        }

        // ******************** Teams Area ********************

        [HttpGet("GetAllTeams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Admin/get-all-teams";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Teams = JsonConvert.DeserializeObject<List<Teams>>(content);

                    return View(Teams);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $" API Hatası: {response.StatusCode} - {errorContent}";
                    return View(new List<Teams>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View(new List<Teams>());
            }
        }

        [HttpDelete("DeleteTeams/{id}")]
        public async Task<IActionResult> DeleteTeams(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            var apiUrl = $"https://localhost:7107/api/Admin/delete-team-by-id/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Delete Teams ";
                return RedirectToAction("GetAllEvaluationCriteria");
            }
            else
            {
                ViewBag.ErrorMessage = "Delete Teams Error.";
                return RedirectToAction("GetAllEvaluationCriteria");
            }
        }

        [HttpGet("PresentationTrigger")]
        public IActionResult PresentationTrigger()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            return  View();
        }

        [HttpPost("DateApprove")]
        public async Task<IActionResult> DateApprove()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            var apiUrl = "https://localhost:7107/api/Manuel/schedule-teams-presentations-optimized-backtracking";

            // Token alımı
            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Authorization token is missing." });
            }

            try
            {
                using (var client = new HttpClient())
                {
                    // Token'i Authorization Header'a ekliyoruz
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    // API'ye POST isteği gönderiyoruz
                    var response = await client.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(new { message = "Operation completed successfully." });
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, new { message = "API request failed.", details = errorContent });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost("ProfessorAndUserSync")]
        public async Task<IActionResult> ProfessorAndUserSync()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            var apiUrl = "https://localhost:7107/api/Manuel/sync-users-with-professors";

            // Token alımı
            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Authorization token is missing." });
            }

            try
            {
                using (var client = new HttpClient())
                {
                    // Token'i Authorization Header'a ekliyoruz
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    // API'ye POST isteği gönderiyoruz
                    var response = await client.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(new { message = "Operation completed successfully." });
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, new { message = "API request failed.", details = errorContent });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


    }
    
}

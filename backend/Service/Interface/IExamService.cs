﻿using backend.Entities;

namespace backend.Service.Interface
{
    public interface IExamService 
    {
        Task<int> EndExam(List<UserAnswer> userAnswers, int examId, int userId);
        Task<dynamic> GetDetailsExam(int examId);
        Task StartExam(int examId, int userId);
        Task<Exam> CreateAsync(Exam exam,int index);
        Task<List<Exam>> GetAllAsync();
        Task<Exam?> GetByIdAsync(int id);
        Task<Exam?> UpdateAsync(int id, Exam updatedItem);
        Task<bool> DeleteAsync(int id);
    }
}

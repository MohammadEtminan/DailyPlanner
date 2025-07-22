using CommunityToolkit.Mvvm.Input;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Application.State;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DailyPlanner.Application.Interfaces.Repositories;

namespace DailyPlanner.Presentation.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly ICurrentUserStore _currentUserStore;
        private readonly ITaskRepository _taskRepository;

        public ObservableCollection<TaskItem> Tasks { get; } = new();

        [ObservableProperty]
        private string _newTaskTitle = string.Empty;

        public DashboardViewModel(ICurrentUserStore currentUserStore, ITaskRepository taskRepository)
        {
            _currentUserStore = currentUserStore;
            _taskRepository = taskRepository;
        }

        // کامندی برای بارگذاری وظایف وقتی صفحه نمایش داده می‌شود
        [RelayCommand]
        private async Task LoadTask()
        {
            if (!_currentUserStore.IsLoggedIn) { return; }
            
            Tasks.Clear();
            var tasks = await _taskRepository.GetByUserIdAsync(_currentUserStore.CurrentUser!.Id);
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }

        // کامندی برای افزودن یک وظیفه جدید
        [RelayCommand]
        private async Task AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTaskTitle) || !_currentUserStore.IsLoggedIn) return;

            var newTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = NewTaskTitle,
                DueDate = DateTime.Now.AddDays(1),
                UserId = _currentUserStore.CurrentUser!.Id
            };

            await _taskRepository.AddAsync(newTask);
            Tasks.Add(newTask);
            NewTaskTitle = string.Empty;            
        }

        // کامندی برای حذف یک وظیفه
        [RelayCommand]
        private async Task DeleteTask(TaskItem task)
        {
            if(task == null) return;
            await _taskRepository.DeleteAsync(task.Id);
            Tasks.Remove(task);
        }

        // کامندی برای تغییر وضعیت تکمیل یک وظیفه
        [RelayCommand]
        private async Task ToggleTaskStatus(TaskItem task)
        {
            if (task == null) return;
            task.IsCompleted = !task.IsCompleted;
            await _taskRepository.UpdateAsync(task);

            // برای به‌روزرسانی UI، باید یک رفرش کوچک انجام دهیم
            var index = Tasks.IndexOf(task);
            Tasks[index] = task;
        }
    }
}
using StudentManagement.Commands;
using StudentManagement.Components;
using StudentManagement.Services;
using StudentManagement.Utils;
using StudentManagement.Models;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading.Tasks;
using StudentManagement.Objects;
using System.Collections.ObjectModel;

namespace StudentManagement.ViewModels
{
    public class LoginViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly ErrorBaseViewModel _errorBaseViewModel;

        private string _oTP;
        public string OTP { get => _oTP; set => _oTP = value; }

        private string _timeCountDown;
        public string TimeCountDown
        {
            get => _timeCountDown;
            set
            {
                _timeCountDown = value;
                OnPropertyChanged();
            }
        }

        private bool _isGetCode;
        public bool IsGetCode
        {
            get => _isGetCode;
            set
            {
                _isGetCode = value;
                OnPropertyChanged();
            }
        }


        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Username))
                {
                    _errorBaseViewModel.AddError(nameof(Username), "Vui lòng nhập tên đăng nhập!");
                }

                OnPropertyChanged();
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Password))
                {
                    _errorBaseViewModel.AddError(nameof(Password), "Vui lòng nhập mật khẩu!");
                }

                OnPropertyChanged();
            }
        }
        private string _gmail;
        public string Gmail
        {
            get => _gmail;
            set
            {
                _gmail = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Gmail))
                {
                    _errorBaseViewModel.AddError(nameof(Gmail), "Vui lòng nhập gmail!");
                }
                if (!IsValidEmail(Gmail))
                {
                    _errorBaseViewModel.AddError(nameof(Gmail), "Địa chỉ mail không đúng định dạng!");
                }
                OnPropertyChanged();
            }
        }
        private string _oTPInView;
        public string OTPInView
        {
            get => _oTPInView;
            set
            {
                _oTPInView = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(OTPInView))
                {
                    _errorBaseViewModel.AddError(nameof(OTPInView), "Vui lòng nhập mã OTP!");
                }

                OnPropertyChanged();
            }
        }
        private string _newPassWord;
        public string NewPassWord
        {
            get => _newPassWord;
            set
            {
                _newPassWord = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(NewPassWord))
                {
                    _errorBaseViewModel.AddError(nameof(NewPassWord), "Vui lòng nhập mật khẩu mới!");
                }

                OnPropertyChanged();
            }
        }
        private string _reNewPassWord;
        public string ReNewPassWord
        {
            get => _reNewPassWord;
            set
            {
                _reNewPassWord = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(ReNewPassWord))
                {
                    _errorBaseViewModel.AddError(nameof(ReNewPassWord), "Vui lòng nhập lại mật khẩu mới!");
                }
                if (IsValid(ReNewPassWord))
                {
                    if (!ReNewPassWord.Equals(NewPassWord))
                    {
                        _errorBaseViewModel.AddError(nameof(ReNewPassWord), "Mật khẩu nhập lại không trùng với mật khẩu mới");
                    }
                }
                OnPropertyChanged();
            }
        }

        public bool HasErrors => _errorBaseViewModel.HasErrors;

        public object CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }

        private object _currentView;

        private bool _isToRemember;
        public bool IsToRemember { get => _isToRemember; set => _isToRemember = value; }

        private Account _rememberedAccount;
        public Account RememberedAccount { get => _rememberedAccount; set { _rememberedAccount = value; } }

        public ICommand SwitchView { get; set; }

        public ICommand GetOTPCodeCommand { get => _getOTPCodeCommand; set => _getOTPCodeCommand = value; }

        private ICommand _getOTPCodeCommand;

        public ICommand ConFirmCommand { get => _conFirmCommand; set => _conFirmCommand = value; }

        private ICommand _conFirmCommand;

        public ICommand RememberUserCommand { get => _remberUserCommand; set => _remberUserCommand = value; }

        private ICommand _remberUserCommand;

        public LoginViewModel()
        {
            try
            {
                IsGetCode = false;
                TimeCountDown = null;
                _errorBaseViewModel = new ErrorBaseViewModel();
                _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
                InitRememberedAccount();

                if (RememberedAccount != null)
                {
                    Username = RememberedAccount.UserName;
                    Password = RememberedAccount.PassWord;
                }

                SwitchView = new RelayCommand<object>((p) => true, (p) => SwitchViewForm());
                GetOTPCodeCommand = new RelayCommand<object>((p) => true, async (p) => await GetOPTAsync());
                ConFirmCommand = new RelayCommand<object>((p) => true, (p) => ConFirm());
                RememberUserCommand = new RelayCommand<object>((p) => true, (p) => RememberUser());
            }
            catch
            {
                MyMessageBox.Show("Có một số lỗi phát sinh", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            
        }
        public void ResetView()
        {
            Gmail = "";
            NewPassWord = null;
            ReNewPassWord = null;
            OTPInView = null;
            Password = null;
            _errorBaseViewModel.ClearAllErrors();
            IsGetCode = false;
            SwitchViewForm();
        }
        public void ConFirm()
        {
            try
            {
                OTPServices.Instance.DeleteOTPOverTime();
                if (!OTPServices.Instance.CheckGetOTPFromEmail(Gmail, SHA256Cryptography.Instance.EncryptString(OTPInView)))
                {
                    MyMessageBox.Show("Mã xác nhận không chính xác", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
                if (UserServices.Instance.ChangePassWord(NewPassWord, Gmail))
                {
                    MyMessageBox.Show("Cập nhật mật khẩu thành công", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    ResetView();
                }
                else
                {
                    MyMessageBox.Show("Cập nhật mật khẩu thất bại", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc cập nhật mật khẩu mới", "Thông báo", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Error);
            }
        

        }
        public string RandomOTP()
        {
            Random generator = new Random();
            return generator.Next(0, 1000000).ToString("D6");
        }
        public void StartCountdown()
        {
            IsGetCode = true;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            TimeCountDown = "60 Giây";
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }
        public async Task SetupAndSendOTPForEmailAsync()
        {
            var body = File.ReadAllText("../../Resources/mail.html");
            var from = new MailAddress("stumanit008@gmail.com", "Stuman Development Team");
            var to = new MailAddress(Gmail.Trim());
            MailMessage mm = new MailMessage(from, to)
            {
                Subject = OTP + " là mã khôi phục tài khoản Stuman của bạn",
                IsBodyHtml = true,
                Body = body.Replace("OTP_CODE", OTP),
                Priority = MailPriority.High
            };
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(Properties.Settings.Default.Email, Properties.Settings.Default.Password),
                EnableSsl = true
            };
            try
            {
                await smtp.SendMailAsync(mm);
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra, không thể gửi mã OTP", "OTP", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public async Task GetOPTAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Gmail))
                {
                    MyMessageBox.Show("Cần phải nhập địa chỉ mail trước khi lấy mã", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
                if (!IsValidEmail(Gmail))
                {
                    MyMessageBox.Show("Địa chỉ mail không hợp lệ", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }

                StartCountdown();

                OTP = RandomOTP();


                await OTPServices.Instance.SaveOTP(Gmail, SHA256Cryptography.Instance.EncryptString(OTP));

                await SetupAndSendOTPForEmailAsync();
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc tạo OTP", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var tmp = Convert.ToInt32(TimeCountDown.Split(' ')[0]);
            tmp -= 1;
            TimeCountDown = tmp.ToString() + " Giây";
            if (tmp == 0)
            {
                (sender as DispatcherTimer).Stop();
                TimeCountDown = null;
            }

        }

        bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void SwitchViewForm()
        {
            CurrentView = (CurrentView == null) || (CurrentView.GetType() == typeof(LoginForm)) ? new ForgotPassword() : (object)new LoginForm();
        }

        public bool IsExistAccount()
        {
            try
            {
                if (LoginServices.Instance.IsUserAuthentic(Username, Password))
                {
                    LoginServices.Instance.Login(Username);
                    return true;
                }

                _ = MyMessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!\nVui lòng thử lại!", "Đăng nhập thất bại", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
            catch
            {
                return false;
                MyMessageBox.Show("Đã có lỗi trong việc xác nhận tài khoản", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            
            /*try
            {
                
            }
            catch
            {
                _ = MyMessageBox.Show("Xảy ra lỗi kết nối đến cơ sở dữ liệu!\nVui lòng thử lại!", "Đăng nhập thất bại", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }*/
        }

        public void RememberUser()
        {
            if (!IsToRemember)
            {
                /*if (RememberedAccount == null)
                    return;
                // Use Remembered Account to input textboxes
                if (!LoginServices.Instance.IsUserAuthentic(Username, Password))
                {
                    // Tự động đăng nhập nếu txbName trống, hoặc txbName trùng Username remembered
                    if (Username == null || Username == "" || Username.ToLower() == RememberedAccount.UserName.ToLower())
                    {
                        Username = RememberedAccount.UserName;
                        Password = RememberedAccount.PassWord;
                    }
                    return;
                }*/
            }
            else
            {
                // Note Remember Account to RAM and disk
                if (LoginServices.Instance.IsUserAuthentic(Username, Password))
                {
                    try
                    {
                        RememberedAccount = new Account(Username, Password);
                        using (StreamWriter sw = new StreamWriter(LoginServices.FilePathRememberedAccount))
                        {
                            sw.Write(RememberedAccount.UserName + '\t' + LoginServices.Encrypt(RememberedAccount.PassWord, "S7uMan"));
                        }
                    }
                    catch { }
                }
            }





        }
        public void InitRememberedAccount()
        {
            RememberedAccount = null;
            string filePath = LoginServices.FilePathRememberedAccount;
            if (File.Exists(filePath))
            {
                string fileContent = "";
                using (StreamReader sr = new StreamReader(filePath))
                {
                    fileContent = sr.ReadToEnd();
                    string accountRow = fileContent.Split('\n')[0];
                    if (accountRow == "")
                        return;
                    string[] account = accountRow.Split('\t');
                    RememberedAccount = new Account(account[0], LoginServices.Decrypt(account[1], "S7uMan"));
                }
            }
            else
            {
                //File.CreateText(LoginServices.FilePathRememberedAccount);
            }
        }

        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }
    }
}

var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
function login() {
    return __awaiter(this, void 0, void 0, function* () {
        const formLogin = document.getElementById('form-login');
        const passwordForm = document.getElementById('password-hide');
        formLogin.addEventListener('submit', (event) => __awaiter(this, void 0, void 0, function* () {
            event.preventDefault();
            const email = document.getElementById('input-login');
            const emailTarget = email.value;
            const response = yield fetch(`/Account/GetRole?email=${emailTarget}`);
            const data = yield response.json();
            if (data == 'Customer' || data == "NotFound") {
                formLogin.submit();
            }
            else {
                if (passwordForm.style.display === 'none')
                    passwordForm.style.display = 'block';
                else
                    formLogin.submit();
            }
        }));
    });
}
login();
//# sourceMappingURL=login.js.map
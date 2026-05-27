async function login()
{
    const formLogin = document.getElementById('form-login') as HTMLFormElement;
    const passwordForm = document.getElementById('password-hide') as HTMLDivElement;

    formLogin.addEventListener('submit', async(event) => {
        event.preventDefault();
        const email = document.getElementById('input-login') as HTMLInputElement;
        const emailTarget = email.value;

        const response = await fetch(`/Account/GetRole?email=${emailTarget}`);
        const data = await response.json();

        if (data == 'Customer' || data == "NotFound")
        {
            formLogin.submit();
        }
        else
        {
            if(passwordForm.style.display === 'none')
                passwordForm.style.display = 'block';
            else
                formLogin.submit();
        }

    })
}

login();


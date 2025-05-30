const form = document.getElementById('contactForm');
form.addEventListener('submit',()=>{
const name = document.getElementById('name');
const email = document.getElementById('email');
const phone = document.getElementById('phone');
const message = document.getElementById('message');
fetch('ip/weather/GetWeatherForecast',{
    headers:{
    },
    method: "POST",
    body:JSON.stringify({
        name:name.value,
        email:email.value,
        phone:phone.value,
        message:message.value
    })
})
});
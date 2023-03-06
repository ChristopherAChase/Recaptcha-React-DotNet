import React from "react";
import fromUTF8Array from "../Utilities/UTF8ArrayConverter";

const RecaptchaButton = ({formId}) => {
  let grecaptcha = window.grecaptcha;
  const SITE_KEY = '6LcSadEkAAAAAHB4_1lNEGWSAJxV1kNiq4ZZsFzJ'; 

  const onRecaptchaSubmit = (event) => {
    event.preventDefault();

      console.log("getting ready to perform recaptcha validation...");
      grecaptcha.execute(SITE_KEY, {action: "submit"})
        .then((token) => {
          console.log(`Googles response token: ${token}`);
          verifyRecaptchaToken(token)
        }, 
        (reason) => {
          console.error(reason);
          grecaptcha.reset()
        });
        
  }

  const createRecaptchaVerifyUrl = (recaptchaToken) => {
    
    let recaptchaVerifyUrlParameters = new URLSearchParams([...new FormData(document.getElementById(formId)).entries()])
    recaptchaVerifyUrlParameters.append('RecaptchaToken', recaptchaToken)
    
    return new URL(`https://localhost:7192/api/Recaptcha/Verify?${recaptchaVerifyUrlParameters}`);
  }

  async function verifyRecaptchaToken(recaptchaToken){
    console.log("Sending token to backend");
    
    const recaptchaVerifyUrl = createRecaptchaVerifyUrl(recaptchaToken);
    
    fetch(recaptchaVerifyUrl, {
      method: "POST", 
      body: recaptchaVerifyUrl.searchParams,
    }).then((response) => {
      if(!response.ok) {
        const errorBuild = { 
          type: "Error", 
          message: response.message || "something went wrong", 
          data: response.data || "", 
          code: response.code || ""
        }; 

        console.log(`Error: ${JSON.stringify(response.body)}`);
        return 
      }

      response.body 
        .getReader()
        .read()
        .then(({done, value}) => {
          let converted = fromUTF8Array(value);
          console.log(`Validation status from server: ${converted}`);
        })
    })
  }




  const onRecaptchaResponse = (token) => { 
    console.log(`Recaptcha has been submitted! Token: ${token}`);
  }

  return (
    <button 
      onClick={onRecaptchaSubmit}
    >
      Submit
    </button>
  )
};

export default RecaptchaButton;
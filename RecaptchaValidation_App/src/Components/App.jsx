import React, { Component } from 'react';
import BasicForm from './BasicForm';
import TextInput from './TextInput';
import RecaptchaButton from './RecaptchaButton';


const App = () => {
  return (
    <BasicForm formId="recaptcha-test-form">
      <TextInput inputType='text' inputId='firstName' inputName="firstName" isRequired={true} labelText='First Name:'></TextInput>
      <TextInput inputType='text' inputId='lastName' inputName="lastName" isRequired={true} labelText='Last Name:'></TextInput>
      <RecaptchaButton formId="recaptcha-test-form"/>
    </BasicForm>
  )
}

export default App;
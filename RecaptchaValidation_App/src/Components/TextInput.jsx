const TextInput = ({inputId, inputType, inputName, isRequired, labelText = inputName}) => {
  return (
    <>
      <label htmlFor={inputName} className="formLabel">
        {labelText ? labelText : inputName}
      </label>
      <input name={inputName} id={inputId} type={inputType} required={isRequired ? `required` : ``}/>
    </>
  )
};

export default TextInput;
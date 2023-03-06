const BasicForm = ({formId, children}) => { 
  return (
    <form id={formId} action="" method="POST">
      {children}
    </form>
  )
};

export default BasicForm;
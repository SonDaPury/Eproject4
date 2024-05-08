import { setUsername } from "../redux/slices/exampleSlice.js";
import { useDispatch, useSelector } from "react-redux";
import { exampleSelector } from "../redux/selectors.js";

const ExampleRedux = () => {
  const data = useSelector(exampleSelector);
  const dispatch = useDispatch();
  console.log(data);
  return (
    <div>
      <h1>Example redux</h1>
      <button
        onClick={() => {
          dispatch(setUsername("John Doe"));
        }}>
        Dispatch
      </button>
    </div>
  );
};

export default ExampleRedux;

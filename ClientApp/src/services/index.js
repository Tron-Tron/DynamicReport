export const GetTableInfos = () => {
  var requestOptions = {
    method: "GET",
    redirect: "follow",
  };

  return fetch(
    "https://localhost:44309/api/TableInfor/GetTableInfos",
    requestOptions
  )
    .then((response) => response.json())
    .then((result) => result)
    .catch((error) => {
      console.log("error", error);
      return [];
    });
};

export const RenderReport = (data) => {
  var requestOptions = {
    method: "POST",
    redirect: "follow",
    body: JSON.stringify(data),
    headers: {
      "Content-Type": "application/json",
    },
  };

  return fetch(
    "https://localhost:44309/api/TableInfor/RenderReport",
    requestOptions
  )
    .then((response) => response.text())
    .then((result) => result)
    .catch((error) => {
      console.log("error", error);
      return null;
    });
};
export const GetStringQuery = (data) => {
  var requestOptions = {
    method: "POST",
    redirect: "follow",
    body: JSON.stringify(data),
    headers: {
      "Content-Type": "application/json",
    },
  };

  return fetch(
    "https://localhost:44309/api/TableInfor/GetStringQuery",
    requestOptions
  )
    .then((response) => response.text())
    .then((result) => result)
    .catch((error) => {
      console.log("error", error);
      return null;
    });
};

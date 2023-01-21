// csv ファイル読み込み
export function ReadFile(file) {
    const reader = new FileReader();
    return reader.readAsText(file);
}

// ファイルパース
export function mapCSVToArray(csv){
    return csv.split('\n').map((row) => row.split(','));
}